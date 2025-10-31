using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : TowerBase
{
    [Header("Cannon Settings")]
    [SerializeField] private float _explosionRadius = 3.0f;

    protected override void Awake()
    {
        _damage = 3f;
        base.Awake();

        // 인스펙터 연결이 비어있으면 한 번 자동 검색
        if (_bulletManager == null)
        {
            _bulletManager = FindFirstObjectByType<BulletManager>();
            if (_bulletManager == null)
            {
                Debug.LogError("[CannonTower] BulletManager reference is missing. Please assign in Inspector.");
            }
        }
    }
    protected override void AttackTarget()
    {
        if (_target.Count == 0 || Time.time < _nextShot)
        {
            return;
        }
        EffectManager.Instance.PlayEffect(_fireEffectName, _firePoint.position, _firePoint.rotation, transform);
        SoundManager.Instance.Clip("Cannon");
        GameObject _bullet = _bulletManager.MakeBullet(_bulletType);
        if (_bullet != null)
        {
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            CannonBall _setBulletComponent = _bullet.GetComponent<CannonBall>();
            if (_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(_damage);
                _setBulletComponent.SetRadius(_explosionRadius);
                _setBulletComponent.Shoot();
            }
            _nextShot = Time.time + _shotDelay;
        }


        

    }
}
