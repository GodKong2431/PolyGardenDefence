using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class CannonBall : BulletBase
{
    [SerializeField] float _explosionRadius;
    
    protected override void OnTriggerEnter(Collider other)
    {
        Explode();        
    }

    
    private void Explode()
    {
        Collider[] _hitEnemies;
        _hitEnemies = Physics.OverlapSphere(transform.position, _explosionRadius);        
        foreach (var hit in _hitEnemies)
        {
            //대포알 스스로는 감지하지 않기
            if (hit.gameObject.CompareTag("Bullet"))
            {
                continue; 
            }        

            if (hit != null)
            {
                GiveDamage(hit);
                Debug.Log("맞은놈 : " + hit.name);
            }
        }
        OffBullet();
    }

    //폭발반경 확인
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
