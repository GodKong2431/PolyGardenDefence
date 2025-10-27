using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : TowerBase
{
    [SerializeField] private float explosionRadius = 3.0f;

    protected override void Awake()
    {
        damage = 3f;
        base.Awake();
    }
    protected override void AttackTarget()
    {
        if (target.Count == 0 || Time.time < nextShot)
        {
            return;
        }
        nextShot = Time.time + shotDelay;

        foreach (var bullet in bulletPool)
        {
            if (bullet.activeSelf == false)
            {
                BE setBulletComponent = bullet.GetComponent<BE>();

                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                setBulletComponent.SetDamage(damage);
                //setBulletComponent.SetRadius(explosionRadius);
                bullet.SetActive(true);
                return;
            }

        }
    }
}
