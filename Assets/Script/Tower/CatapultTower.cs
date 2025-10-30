using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultTower : TowerBase
{
    [SerializeField] private float _criticalChance = 0.2f; //치명타 확률
    [SerializeField] private float _criticalMultiplier = 2.0f; //치명타 배율
    [SerializeField] private Animator _animator;
    
    protected override void Awake()
    {
        
        base.Awake();
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

        float criticalDamage = _damage;

        if (Random.value < _criticalChance)
        {
            criticalDamage *= _criticalMultiplier;
        }

        GameObject _bullet = BulletManager.Instance.MakeBullet(_bulletType);

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
            _nextShot = Time.time + _shotDelay;
        }
    }
}
