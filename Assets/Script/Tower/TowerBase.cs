using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class TowerBase : MonoBehaviour
{
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected float _range;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected string _name = "base tower";
    [SerializeField] protected float _damage = 1f;
    [SerializeField] protected float _shotDelay = 0.5f;
    [SerializeField] protected BulletType _bulletType;
    [SerializeField] protected TowerType _towerType;
    protected int _level = 1; 

    protected float _nextShot = 0;
    protected List<Transform> _target = new List<Transform>();
    protected SphereCollider _sphereCollider;

    protected float _baseShotDelay;
    protected Coroutine _buffCoroutine = null;

    protected virtual void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        SetRange();
        _baseShotDelay = _shotDelay;
    }

    protected virtual void Update()
    {
        SetTarget();
        AttackTarget();
    }
    //공격범위 설정
    protected void SetRange()
    {
        if (_sphereCollider != null)
        {
            _sphereCollider.radius = _range;
        }
    }
    //첫번째 적을 추적하게하는 메서드
    protected virtual void SetTarget()
    {
        for(int i=_target.Count-1;i>=0;i--)
        {
            Transform currentTarget = _target[i];

            if(currentTarget == null || currentTarget.gameObject.activeSelf==false)
            {
                _target.RemoveAt(i);
                
            }
        }

        if (_target.Count > 0)
        {
            Transform firstEnemy = _target[0];

            if (firstEnemy != null)
            {
                transform.LookAt(firstEnemy);
            }
        }
       
    }
    //적 공격
    protected virtual void AttackTarget()
    {
        if(_target.Count == 0 || Time.time < _nextShot)
        {
            return;
        }
        GameObject _bullet = BulletManager.Instance.MakeBullet(_bulletType);

        if(_bullet != null)
        {
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            BulletBase _setBulletComponent = _bullet.GetComponent<BulletBase>();
            if(_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(_damage);
            }
            _nextShot = Time.time + _shotDelay;
        }
    }
  
    public void ApplyAttackSpeedBuff(float amount,float duration)
    {
        if(_buffCoroutine != null)
        {
            StopCoroutine(_buffCoroutine);
            _shotDelay = _baseShotDelay;
        }
        _buffCoroutine = StartCoroutine(AttackSpeedBuffCoroutine(amount, duration));
    }

    private IEnumerator AttackSpeedBuffCoroutine(float amount,float duration)
    {
        _shotDelay *= (1f - amount);

        yield return new WaitForSeconds(duration);

        _shotDelay = _baseShotDelay;
        _buffCoroutine = null;
    }

    //콜라이더로 들어오는 적 순서대로 리스트에 저장
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Transform enemyTransform = other.transform;

            if (!_target.Contains(enemyTransform))
            {
                _target.Add(enemyTransform);
            }
        }
    }
    //콜라이더 나가는 순서대로 제거
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Transform enemyTransform = other.transform;

            if (_target.Contains(enemyTransform))
            {
                _target.Remove(enemyTransform);
            }
        }
    }


}

