using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaTower : TowerBase
{


    [Header("Ballista Settings")]
    [SerializeField] private int _pierceCount = 2; //관통 횟수

    protected override void Awake()
    {
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
        if (_target.Count == 0 || Time.time < _nextShot)
        {
            return;
        }
        //EffectManager.Instance.PlayEffect("ASD",_firePoint.position, _firePoint.rotation,transform);
        SoundManager.Instance.Clip("Arrow");
        GameObject _bullet = _bulletManager.MakeBullet(_bulletType);

        if (_bullet != null)
        {
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            Arrow _setBulletComponent = _bullet.GetComponent<Arrow>();
            if (_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(_damage);
                _setBulletComponent.SetPiercing(_pierceCount);
                _setBulletComponent.Shoot();
            }
            _nextShot = Time.time + _shotDelay;
        }


        

    }
}
