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

        _nextShot = Time.time + _shotDelay;

        float criticalDamage = _damage;

        if (Random.value < _criticalChance)
        {
            criticalDamage *= _criticalMultiplier;
        }

        //foreach (var bullet in _bulletPool)
        //{
        //    if (bullet.activeSelf == false)
        //    {
        //        BE setBulletComponent = bullet.GetComponent<BE>();
        //
        //        bullet.transform.position = _firePoint.position;
        //        bullet.transform.rotation = _firePoint.rotation;
        //        setBulletComponent.SetDamage(criticalDamage);
        //        bullet.SetActive(true);
        //        return;
        //    }
        //
        //}
    }
}
