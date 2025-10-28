using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaTower : TowerBase
{
    [SerializeField] private int _pierceCount = 2; //관통 횟수

    protected override void Awake()
    {
        // 사거리가 길고 관통하는 대신 딜레이도 길게
        _damage = 5f;
        _range = 10f;
        _shotDelay = 1.5f;
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
        //        //setBulletComponent.SetPiercing(pierceCount);
        //        bullet.SetActive(true);
        //        return;
        //    }
        //
        //}
    }
}
