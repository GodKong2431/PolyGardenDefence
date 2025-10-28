using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : BulletBase
{
    [SerializeField] float _explosionRadius;
    float _detectRange = 0;
    
        
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Bullet"))
        {
            return;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();            
        }
        OffBullet();
    }

    private void Explode()
    {
        Collider[] _hitEnemies;
        _hitEnemies = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var hit in _hitEnemies)
        {
            //대포 및 대포알이나 다른 총알은 감지하지 않기.
            if (hit.gameObject.CompareTag("Bullet") || hit.gameObject.CompareTag("Tower"))
            {
                continue;
            }
            if (hit != null)
            {
                GiveDamage(hit);
                Debug.Log("맞은놈 : " + hit.name);                
            }
            OffBullet();
        }
    }


    //폭발반경 확인
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
