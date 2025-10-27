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
            if (hit.gameObject == gameObject)
            {
                continue; 
            }
            EnemyBase enemy = hit.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                //GiveDamage(enemy);
                Debug.Log("¸ÂÀº³ð : " + hit.name);
            }
        }
        OffBullet();
    }

    //Æø¹ß¹Ý°æ È®ÀÎ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
