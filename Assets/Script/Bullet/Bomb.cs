using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    [SerializeField] float _explosionRadius;
    
    protected override void OnTriggerEnter(Collider other)
    {
       Explode();
       gameObject.SetActive(false);

    }    

    private void Explode()
    {
        Collider[] _hitEnemies;
        _hitEnemies = Physics.OverlapSphere(transform.position, _explosionRadius);
        //Enemy enemy = hit.GetComponent<Enemy>();
        foreach (var hit in _hitEnemies)
        {
            //GiveDamage(enemy);
        }
    }
}
