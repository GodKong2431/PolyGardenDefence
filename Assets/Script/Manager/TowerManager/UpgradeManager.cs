using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class UpgradeManager : SingleTon<UpgradeManager>
{
    TowerType SelectedType { get; set; }
    public GameObject BuildTower() // 타입에 맞는 타워 생성.
    {        
        GameObject newTower = null;
        newTower = Instantiate(TowerStorage.Instance.BasicTowers[SelectedType]);        
        return newTower;
    }

    public void PlaceTower(Vector3 position) //위치값 받아서 타워 배치.
    {
        GameObject newTower = BuildTower();
        newTower.transform.position = position;
        GameManager.Instance.SubGold(newTower.GetComponent<TowerBase>().Price);
    }

    public GameObject UpgradeTower(GameObject tower)//기존 타워 업그레이드 매서드.
    {
        GameObject upgradedTower = null;
        TowerBase towerBase = tower.GetComponent<TowerBase>();
        TowerType type = towerBase.TowerType;
        int level = towerBase.Level;
        switch (level)
        {
            case 0:
                upgradedTower = Instantiate(TowerStorage.Instance.AdvancedTowers[type], tower.transform.position, tower.transform.rotation);
                break;
            case 1:
                upgradedTower = Instantiate(TowerStorage.Instance.FinalTowers[type], tower.transform.position, tower.transform.rotation);
                break;
            case 2:
                Debug.Log("이미 최고 레벨인 타워입니다!");
                return null;
            default:
                Debug.Log("잘못된 타워 레벨 설정입니다.");
                return null;
        }
        Destroy(tower);
        return upgradedTower;
    }

    public void SellTower(GameObject tower)
    {
        TowerBase towerBase = tower.GetComponent<TowerBase>();
        if (towerBase != null)
        {
            Destroy(tower);
            GameManager.Instance.AddGold(towerBase.Price);
        }
    }
}
