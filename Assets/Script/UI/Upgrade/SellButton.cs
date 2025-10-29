using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour
{
    TowerBase _selectTower;
    private void Awake()
    {
        _selectTower = GetComponentInParent<UpgradeUI>().SelectTower;
    }

    public void OnClickSell()
    {
        UpgradeManager.SellTower(_selectTower);
    }
}
