using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BulletBase
{    
    private int _pierceCount;

    protected override void OnEnable()
    {        
        base.OnEnable();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Tower"))
        {
            return;
        }
        else if (other.CompareTag("Enemy"))
        {
            _pierceCount = Mathf.Max(_pierceCount - 1, 0);         
            if (_pierceCount == 0)
            {
                GiveDamage(other);
                OffBullet();
            }
        }
        
    }

    public void SetPiercing(int pierceCount)
    {
        _pierceCount = pierceCount;
    }
}
