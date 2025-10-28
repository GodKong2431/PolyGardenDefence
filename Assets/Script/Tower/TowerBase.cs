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

    protected float _nextShot = 0;
    protected List<Transform> _target = new List<Transform>();

    protected SphereCollider _sphereCollider;
    //protected GameObject[] _bulletPool;
    //[SerializeField] protected int _bulletPoolSize = 15;
    [SerializeField] protected BulletType _bulletType;


    protected virtual void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        //Init();
        SetRange();
    }

    protected void Update()
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
    protected void SetTarget()
    {
        for(int i = 0; i < _target.Count; i++)
        {
            Transform currentTarget = _target[i];

            if(currentTarget == null || currentTarget.gameObject.activeSelf==false)
            {
                _target.RemoveAt(i);
                i--;
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
            //if(_setBulletComponent != null)
            //{
            //    _setBulletComponent.SetDamage(_damage);
            //}
            _nextShot = Time.time + _shotDelay;
        }

        //foreach (var bullet in _bulletPool)
        //{
        //    if (bullet.activeSelf == false)
        //    {
        //        BulletBase setBulletComponent = bullet.GetComponent<BulletBase>();
        //
        //        bullet.transform.position = _firePoint.position;
        //        bullet.transform.rotation = _firePoint.rotation;
        //        //setBulletComponent.SetDamage(_damage);
        //        bullet.SetActive(true);
        //        return;
        //    }
        //
        //}
    }

    //오브젝트 풀링으로 총알 미리생성, 후에 매니저로 이동
    //protected void Init()
    //{
    //    _bulletPool = new GameObject[_bulletPoolSize];
    //
    //    for (int i = 0; i < _bulletPool.Length; i++)
    //    {
    //        _bulletPool[i] = Instantiate(_bulletPrefab);
    //        _bulletPool[i].SetActive(false);
    //    }
    //}

    //콜라이더로 들어오는 적 순서대로 리스트에 저장
    protected void OnTriggerEnter(Collider other)
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
    protected void OnTriggerExit(Collider other)
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

