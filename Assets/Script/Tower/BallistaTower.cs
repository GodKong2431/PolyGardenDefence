using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaTower : TowerBase
{
    [SerializeField] private int pierceCount = 2; //관통 횟수

    protected override void Awake()
    {
        // 사거리가 길고 관통하는 대신 딜레이도 길게
        damage = 5f;
        range = 10f;
        shotDelay = 1.5f;
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
                //setBulletComponent.SetPiercing(pierceCount);
                bullet.SetActive(true);
                return;
            }

        }
    }
}
