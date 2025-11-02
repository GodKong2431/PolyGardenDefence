using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : BulletBase
{
    private int _pierceCount;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        if (_pierceCount <= 0)
        {
            OffBullet();
        }
        if (_target != null && _target.GetComponent<EnemyBase>().IsDead)
        {
            OffBullet();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.CompareTag("Bullet") && !other.CompareTag("Tower"))
        {
            _pierceCount = Mathf.Max(_pierceCount - 1, 0);
            GiveDamage(other);
            if (_pierceCount == 0)
            {
                OffBullet();
            }
        }

    }

    public void SetPiercing(int pierceCount)
    {
        _pierceCount = pierceCount;
    }
}
