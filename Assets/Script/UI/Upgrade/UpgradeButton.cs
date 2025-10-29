using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    GameObject _selectTower;
    private void Awake()
    {
        _selectTower = GetComponentInParent<UpgradeUI>().SelectTower;
    }

    public void OnClickUpgrade()
    {
        //UpgradeManager.Upgrade(_selectTower);
    }

}
