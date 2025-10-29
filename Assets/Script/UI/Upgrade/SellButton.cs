using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour
{
    TowerBase _selectTower;

    public void OnClickSell()
    {
        _selectTower = GetComponentInParent<UpgradeUI>().SelectTower;
        UpgradeManager.Instance.SellTower(_selectTower);
    }
}
