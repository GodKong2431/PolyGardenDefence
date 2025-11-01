using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTower : TowerBase
{
    [SerializeField] protected TowerBaseStatsSO _TurretStats;
    public TowerBaseStatsSO TurretStats => _TurretStats;
    protected override void Awake()
    {
        Stats = _TurretStats;
        base.Awake();
        
        // 인스펙터 연결이 비어있으면 한 번 자동 검색
        if (_bulletManager == null)
        {
            _bulletManager = FindFirstObjectByType<BulletManager>();
            if (_bulletManager == null)
            {
                Debug.LogError("[TurretTower] BulletManager reference is missing. Please assign in Inspector.");
            }
        }
    }
}
