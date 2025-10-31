using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeManager : SingleTon<UpgradeManager>
{    
    public GameObject BuildTower(TowerType selectedType) // 타입에 맞는 타워 생성.
    {        
        GameObject newTower = null;
        newTower = Instantiate(TowerStorage.Instance.BasicTowers[selectedType]);        
        return newTower;
    }

    public void PlaceTower(TowerType type, GameObject tile) //타일 받아서 타워 배치.
    {
        GameObject newTower = BuildTower(type);
        TowerSpot spot = tile.GetComponentInChildren<TowerSpot>();
        if (spot.PlacedTower == null)
        {            
            if (GameManager.Instance.SubGold(newTower.GetComponentInChildren<TowerBase>().Price) == true)
            {                
                newTower.transform.position = spot.GetPlacePosition();
                spot.Occupy(newTower);
                newTower.GetComponentInChildren<TowerBase>().Tile = tile;
            }
            else
            {
                Debug.Log("타워를 설치할 비용이 부족합니다.");
                RemoveTower(newTower.GetComponentInChildren<TowerBase>());
            }
                
        }
        else
        {
            Debug.Log("이미 타워가 배치된 타일입니다.");
            //경우에 따라 알림 메시지용 UI 호출.
        }        
    }

    public GameObject UpgradeTower(TowerBase selectedTower)//기존 타워 업그레이드 매서드.
    {
        int cost;
        GameObject upgradedTower = null;
        TowerType type = selectedTower.TowerType;
        TowerSpot selectedTile = selectedTower.Tile.GetComponent<TowerSpot>();
        int level = selectedTower.Level;

        switch (level)
        {
            case 1:
                upgradedTower = Instantiate(TowerStorage.Instance.AdvancedTowers[type], selectedTile.GetPlacePosition(), selectedTile.transform.rotation);                
                break;
            case 2:
                upgradedTower = Instantiate(TowerStorage.Instance.FinalTowers[type], selectedTile.GetPlacePosition(), selectedTower.transform.rotation);
                break;
            case 3:
                Debug.Log("이미 최고 레벨인 타워입니다!");
                return null;
            default:
                Debug.Log("잘못된 타워 레벨 설정입니다.");
                return null;
        }
        cost = upgradedTower.GetComponentInChildren<TowerBase>().Price - selectedTower.Price;
        
        if (GameManager.Instance.SubGold(cost) == true)
        {
            selectedTile.Occupy(upgradedTower);
            upgradedTower.GetComponentInChildren<TowerBase>().Tile = selectedTower.Tile;
            RemoveTower(selectedTower);
            return upgradedTower;
        }
        else
        {
            Debug.Log("업그레이드 비용이 부족합니다.");
            RemoveTower(selectedTower);
            return null;
        }
    }

    public void SellTower(TowerBase selectedTower)
    {        
        if (selectedTower != null)
        {
            GameManager.Instance.AddGold(selectedTower.Price);
            selectedTower.Tile.GetComponent<TowerSpot>().PlacedTower = null;
            RemoveTower(selectedTower);
        }
    }

    public void RemoveTower(TowerBase tower)
    {
        if (tower.transform.parent.gameObject != null)
        {
            Destroy(tower.transform.parent.gameObject);
        }
        else
        {
            Destroy(tower.gameObject);
        }
    }
}
