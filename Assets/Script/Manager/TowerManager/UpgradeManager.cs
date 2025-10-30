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
        TowerSpot spot = tile.GetComponent<TowerSpot>();
        if (spot.PlacedTower == null)
        {
            GameObject newTower = BuildTower(type);
            newTower.transform.position = spot.GetPlacePosition();
            GameManager.Instance.SubGold(newTower.GetComponent<TowerBase>().Price);
            spot.Occupy(newTower);
        }
        else
        {
            Debug.Log("이미 타워가 배치된 타일입니다.");
            //경우에 따라 알림 메시지용 UI 호출.
        }        
    }

    public GameObject UpgradeTower(TowerBase towerBase)//기존 타워 업그레이드 매서드.
    {
        GameObject upgradedTower = null;        
        TowerType type = towerBase.TowerType;
        int level = towerBase.Level;
        switch (level)
        {
            case 1:
                upgradedTower = Instantiate(TowerStorage.Instance.AdvancedTowers[type], towerBase.transform.position, towerBase.transform.rotation);
                break;
            case 2:
                upgradedTower = Instantiate(TowerStorage.Instance.FinalTowers[type], towerBase.transform.position, towerBase.transform.rotation);
                break;
            case 3:
                Debug.Log("이미 최고 레벨인 타워입니다!");
                return null;
            default:
                Debug.Log("잘못된 타워 레벨 설정입니다.");
                return null;
        }
        Destroy(towerBase.gameObject);        
        towerBase.Tile.GetComponent<TowerSpot>().Occupy(upgradedTower);
        return upgradedTower;
    }

    public void SellTower(TowerBase towerBase)
    {        
        if (towerBase != null)
        {
            Destroy(towerBase.gameObject);
            GameManager.Instance.AddGold(towerBase.Price);
            towerBase.Tile = null;
        }
    }
}
