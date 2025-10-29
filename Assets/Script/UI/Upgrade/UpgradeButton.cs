using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    TowerBase _selectTower;
    private void Awake()
    {
        _selectTower = GetComponentInParent<UpgradeUI>().SelectTower;
    }

    public void OnClickUpgrade()
    {
        UpgradeManager.Instance.SellTower(_selectTower);
    }

}
