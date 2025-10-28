using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultTower : TowerBase
{
    [SerializeField] private float criticalChance = 0.2f; //치명타 확률
    [SerializeField] private float criticalMultiplier = 2.0f; //치명타 배율
    
    
    protected override void Awake()
    {
        damage = 5f;
        range = 10f;
        base.Awake();
    }
    protected override void AttackTarget()
    {
        if(target.Count==0 || Time.time < nextShot)
        {
            return;
        }

        nextShot = Time.time + shotDelay;

        float criticalDamage = damage;

        if (Random.value < criticalChance)
        {
            criticalDamage *= criticalMultiplier;
        }

        foreach (var bullet in bulletPool)
        {
            if (bullet.activeSelf == false)
            {
                BE setBulletComponent = bullet.GetComponent<BE>();

                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                setBulletComponent.SetDamage(criticalDamage);
                bullet.SetActive(true);
                return;
            }

        }
    }
}
