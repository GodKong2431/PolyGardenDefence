using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : BulletBase
{
    private int _pierceCount;
    List<Collider> enemies = null;
    bool isHit;

    protected override void OnEnable()
    {
        base.OnEnable();
        isHit = false;
    }

    protected override void Update()
    {
        if (isHit == false)
        {
            base.Update();            
        }
        
        if (_pierceCount <= 0)
        {
            OffBullet();
        }
        if (isHit == true)
        {
            if (_target != null)
            {                
                transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            }
            else
            {
                OffBullet();
            }
        }
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.CompareTag("Bullet") && !other.CompareTag("Tower"))
        {
            _pierceCount = Mathf.Max(_pierceCount - 1, 0);
            GiveDamage(other);
            isHit = true;
        }
    }

    public void SetPiercing(int pierceCount)
    {
        _pierceCount = pierceCount;
    }
}
