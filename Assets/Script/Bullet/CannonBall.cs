using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : BulletBase
{
    public float _explosionRadius;    
    public string _explosionName = "CannonBallExplosion";
        
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] _hitEnemies;
        _hitEnemies = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var hit in _hitEnemies)
        {            
            if (hit.CompareTag("Enemy"))
            {
                GiveDamage(hit);
                //Debug.Log("¸ÂÀº³ð : " + hit.name);
            }            
        }
        EffectManager.Instance.PlayEffect(_explosionName, transform.position, transform.rotation);
        OffBullet();     
    }

    public void SetRadius(float explosionRadius)
    {
        _explosionRadius = explosionRadius;
    }

    //Æø¹ß¹Ý°æ È®ÀÎ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
