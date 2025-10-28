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
        _nextShot = Time.time + _shotDelay;

        //foreach (var bullet in _bulletPool)
        //{
        //    if (bullet.activeSelf == false)
        //    {
        //        BE setBulletComponent = bullet.GetComponent<BE>();
        //
        //        bullet.transform.position = _firePoint.position;
        //        bullet.transform.rotation = _firePoint.rotation;
        //        setBulletComponent.SetDamage(_damage);
        //        //setBulletComponent.SetRadius(explosionRadius);
        //        bullet.SetActive(true);
        //        return;
        //    }
        //
        //}
    }
}
