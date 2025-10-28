using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : SingleTon<UpgradeManager>
{
    public GameObject BuildTower(TowerType type)
    {
        GameObject newTower = Instantiate(TowerStorage.Instance._basicTowers[type]);
        newTower.transform.position = Vector3.zero;//레이캐스트 위치로 생성
        return newTower;
    }

    //public GameObject UpgradeTower(GameObject tower)
    //{
    //    switch (tower.GetComponent<TowerBase>().)//원래는 레벨 받아야 함
    //    {
    //        case 0:
    //            break;
    //    }

    //}


    
}
