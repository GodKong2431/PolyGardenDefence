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

        GameObject _bullet = BulletManager.Instance.MakeBullet(_bulletType);

        if (_bullet != null)
        {
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            BulletBase _setBulletComponent = _bullet.GetComponent<BulletBase>();
            if (_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(_damage);
                //_setBulletComponent.SetPiercing(_pierceCount);
            }
            _nextShot = Time.time + _shotDelay;
        }


        

    }
}
