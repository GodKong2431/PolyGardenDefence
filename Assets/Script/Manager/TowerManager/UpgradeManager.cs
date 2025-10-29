using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : SingleTon<UpgradeManager>
{
    GameObject CurrentTower { get; set; }
    public GameObject BuildTower(TowerType type) // 타입에 맞는 타워 생성 후 비활성화
    {
        GameObject newTower = null;
        newTower = Instantiate(TowerStorage.Instance.BasicTowers[type]);
        newTower.SetActive(false);
        CurrentTower = newTower;
        return newTower;
    }

    public void PlaceTower(Vector3 position) //위치값 받아서 타워 배치 후 활성화.
    {
        CurrentTower.SetActive(true);
        CurrentTower.transform.position = position;
        //GameManager.Instance.subGold(CurrentTower.GetComponent<TowerBase>._price);
    }

    //public GameObject UpgradeTower(GameObject tower)//기존 타워 업그레이드 매서드.
    //{
    //    GameObject upgradedTower = null;
    //    TowerBase towerBase = tower.GetComponent<TowerBase>();
    //    TowerType type = towerBase.type;
    //    int level = towerBase.level;
    //    switch (level)
    //    {
    //        case 0:
    //            upgradedTower = Instantiate(TowerStorage.Instance.AdvancedTowers[type], tower.transform.position, tower.transform.rotation);
    //            Destroy(tower);
    //            return upgradedTower;
    //        case 1:
    //            upgradedTower = Instantiate(TowerStorage.Instance.FinalTowers[type], tower.transform.position, tower.transform.rotation);
    //            Destroy(tower);
    //            return upgradedTower;
    //        case 2:
    //            Debug.Log("이미 최고 레벨인 타워입니다!");
    //            return null;
    //        default:
    //            Debug.Log("잘못된 타워 레벨 설정입니다.");
    //            return null;
    //    }
    //}

    public void SellTower(GameObject tower)
    {
        TowerBase towerBase = tower.GetComponent<TowerBase>();
        if (towerBase != null)
        {
            Destroy(tower);
            //GameManager.Instance.AddGold(towerBase._price);
        }
    }
}
