using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : SingleTon<UpgradeManager>
{
    public GameObject BuildTower(TowerType type) // 맨땅에 새로운 타워 생성 매서드.
    {
        GameObject newTower = Instantiate(TowerStorage.Instance.BasicTowers[type]);
        newTower.transform.position = Vector3.zero;//레이캐스트 위치로 생성
        return newTower;
    }

    //public GameObject UpgradeTower(GameObject tower)//기존 타워 업그레이드 매서드.
    //{
    //    GameObject newTower = null;
    //    TowerBase towerBase = tower.GetComponent<TowerBase>();
    //    TowerType type = towerBase.type;
    //    int level = towerBase.level; 
    //    switch (level)
    //    {
    //        case 0:
    //            newTower = Instantiate(TowerStorage.Instance.AdvancedTowers[type]);
    //            Destroy(tower);
    //            return newTower;
    //        case 1:
    //            newTower = Instantiate(TowerStorage.Instance.FinalTowers[type]);
    //            Destroy(tower);
    //            return newTower;
    //        case 2:
    //            Debug.Log("이미 최고 레벨인 타워입니다!");
    //            return null;
    //    }
    //}
}
