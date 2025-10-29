using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : TowerBase
{
    [SerializeField] private float _explosionRadius = 3.0f;

    protected override void Awake()
    {
        _damage = 3f;
        base.Awake();
    }
    protected override void AttackTarget()
    {
        if (_target.Count == 0 || Time.time < _nextShot)
        {
            return;
        }
        GameObject _bullet = BulletManager.Instance.MakeBullet(_bulletType);

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
