using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour
{
    public BulletType _type;
    [SerializeField] float _attack;
    [SerializeField] float _speed;
    [SerializeField] protected float _deactiveTime;
    protected float _timeCount;
    protected Rigidbody _rigidBody;



    protected void Awake() //리지드바디 세팅.
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _timeCount = _deactiveTime;
    }

    protected virtual void OnEnable()//활성화하면 날아가기.
    {
        SetBullet();   
        ShootBullet();        
    }

    private void Update()
    {
        CountTime();
    }

    protected void SetBullet()
    {
        _rigidBody.velocity = Vector3.zero;
        _timeCount = _deactiveTime;
    }
    public void SetDamage(float damage)
    {
        _attack = damage;
    }

    protected virtual void OnTriggerEnter(Collider other)//콜라이더에 부딪히면 비활성화.
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GiveDamage(other);
            OffBullet();
        }
        else if (!other.gameObject.CompareTag("Bullet") && !other.gameObject.CompareTag("Tower") && !other.gameObject.CompareTag("TowerPlacementTile"))
        {
            OffBullet();
        }
    }
    private void CountTime()
    {
        _timeCount -= Time.deltaTime;

        if (_timeCount <= 0)
        {
            OffBullet();
        }
    }

    protected virtual void ShootBullet()//날아가는 매서드.
    {
        _rigidBody.AddForce(transform.forward * _speed, ForceMode.Impulse);        
    }
    
    protected void OffBullet()//비활성화 매서드.
    {        
        gameObject.SetActive(false);        
    }    

    protected void GiveDamage(Collider other)
    {
        IDamageable enemy = other.GetComponent<IDamageable>();

        if (enemy != null)
        {
            enemy.ApplyDamage(_attack);
        }
    }    
}
