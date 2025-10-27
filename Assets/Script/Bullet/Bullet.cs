using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum BulletType
{
    Arrow, Ball, Stone, Missile
}

public class Bullet : MonoBehaviour
{
    public BulletType _type { get; set; }
    [SerializeField] float _attack;
    [SerializeField] float _speed;
    [SerializeField] float _deactiveTime;

    private Rigidbody _rigidBody;       

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ShootBullet();        
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //GiveDamage(other.GetComponent<Enemy>())              
        }        
        OffBullet();
    }   

    private void ShootBullet()
    {
        _rigidBody.velocity = transform.forward * _speed;
    }
    
    private void OffBullet()
    {
        _rigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);        
    }    

    protected void GiveDamage()//인자값 Enemy enemy받기
    {
        //enemy.ApplyDamage(_attack);
    }

    
}
