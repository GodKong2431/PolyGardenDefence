using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    TowerBase _selectTower;

    public void OnClickUpgrade()
    {
        _selectTower = GetComponentInParent<UpgradeUI>().SelectTower;
        UpgradeManager.Instance.UpgradeTower(_selectTower);
    }
}
