using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaTower : TowerBase
{
    [SerializeField] private int _pierceCount = 2; //°üÅë È½¼ö

    protected override void Awake()
    {
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

            Arrow _setBulletComponent = _bullet.GetComponent<Arrow>();
            if (_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(_damage);
                _setBulletComponent.SetPiercing(_pierceCount);
            }
            _nextShot = Time.time + _shotDelay;
        }


        

    }
}
