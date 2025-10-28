using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BulletBase
{
    [SerializeField] int _penetNumber;
    int _penetCount;    

    protected override void OnEnable()
    {
        _penetCount = _penetNumber;
        base.OnEnable();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _penetCount = Mathf.Max(_penetCount - 1, 0);         
            if (_penetCount == 0)
            {
                GiveDamage(other);
                OffBullet();
            }
        }
        //else if (!other.gameObject.CompareTag("Bullet"))
        //{
        //    OffBullet();
        //}        
    }
}
