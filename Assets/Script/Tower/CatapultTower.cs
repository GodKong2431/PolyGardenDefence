using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultTower : TowerBase
{

    [Header("Critical Settings")]
    [SerializeField] protected TowerBaseStatsSO _catapultStats;
    public TowerBaseStatsSO CatapultStats => _catapultStats;
    //[SerializeField] private float _criticalChance = 0.2f; //치명타 확률
    //[SerializeField] private float _criticalMultiplier = 2.0f; //치명타 배율

    [Header("Visuals")]
    [SerializeField] private Animator _animator;
    
    protected override void Awake()
    {
        Stats = _catapultStats;
        base.Awake();
        
        // 인스펙터 연결이 비어있으면 한 번 자동 검색
        if (_bulletManager == null)
        {
            _bulletManager = FindFirstObjectByType<BulletManager>();
            if (_bulletManager == null)
            {
                Debug.LogError("[CatapultTower] BulletManager reference is missing. Please assign in Inspector.");
            }
        }
    }
    protected override void AttackTarget()
    {
        

        if(_target.Count==0 || Time.time < _nextShot)
        {
            return;
        }

        Transform targetEnemy = _target[0];
        if (targetEnemy == null || !targetEnemy.gameObject.activeSelf)
        {
            _target.RemoveAt(0);
            return;
        }

        float criticalDamage = Stats._damage;

        if (Random.value < CatapultStats._criticalChance)
        {
            criticalDamage *= CatapultStats._criticalMultiplier;
        }
        
        
        SoundManager.Instance.Clip("Card");
        GameObject _bullet = _bulletManager.MakeBullet(_bulletType);

        if (_bullet != null)
        {
            if(_animator != null)
            {
                _animator.SetTrigger("Attack");
            }
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            BulletBase _setBulletComponent = _bullet.GetComponent<BulletBase>();
            if (_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(criticalDamage);
                _setBulletComponent.Shoot();
            }
            _nextShot = Time.time + Stats._shotDelay;
        }
    }
}
