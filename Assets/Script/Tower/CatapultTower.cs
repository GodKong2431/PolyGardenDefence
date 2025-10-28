using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultTower : TowerBase
{
    [SerializeField] private float _criticalChance = 0.2f; //치명타 확률
    [SerializeField] private float _criticalMultiplier = 2.0f; //치명타 배율
    
    
    protected override void Awake()
    {
        _damage = 5f;
        _range = 10f;
        base.Awake();
    }
    protected override void AttackTarget()
    {
        if(_target.Count==0 || Time.time < _nextShot)
        {
            return;
        }

        float criticalDamage = _damage;

        if (Random.value < _criticalChance)
        {
            criticalDamage *= _criticalMultiplier;
        }

        GameObject _bullet = BulletManager.Instance.MakeBullet(_bulletType);

        if (_bullet != null)
        {
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            BulletBase _setBulletComponent = _bullet.GetComponent<BulletBase>();
            if (_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(criticalDamage);
            }
            _nextShot = Time.time + _shotDelay;
        }
    }
}
