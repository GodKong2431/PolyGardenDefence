using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour, IPoolable
{
    public BulletType _type;
    [SerializeField] float _attack;
    [SerializeField] float _speed;
    [SerializeField] protected float _deactiveTime;
    protected float _timeCount;
    protected Rigidbody _rigidBody;
    Collider _target;


    protected void Awake() //리지드바디 세팅.
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _timeCount = _deactiveTime;
    }

    protected virtual void OnEnable()//활성화하면 날아가기.
    {
        SetBullet();
        //ShootBullet();        
    }

    private void Update()
    {
        CountTime();
        Shoot();
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
        if (other.CompareTag("Enemy"))
        {
            GiveDamage(other);
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
    public void Shoot()
    {
        ShootBullet();
    }
    protected virtual void ShootBullet()//날아가는 매서드.
    {        
        if (_target.enabled == false)
        {
            OffBullet();
            return;
        }
        transform.LookAt(_target.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);

        //_rigidBody.AddForce(transform.forward * _speed, ForceMode.Impulse);
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

    public void GetTarget(Collider target)
    {
        _target = target;
    }

    public void OnGetFromPool()   // 풀에서 꺼낼 때
    {
        SetBullet();
        _rigidBody.velocity = Vector3.zero;
        _timeCount = _deactiveTime;
    }

    public void OnReturnToPool()  // 풀로 반납 직전
    {
        _rigidBody.velocity = Vector3.zero;
        _timeCount = _deactiveTime;
    }
}
