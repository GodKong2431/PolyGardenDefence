using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 꼭 있어야하는 컴포넌트들
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TowerBase : MonoBehaviour
{

    [Header("Pooling / Manager")]
    [SerializeField] protected BulletManager _bulletManager;
    
    


    [Header("Projectile Settings")]
    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected BulletType _bulletType;
    [SerializeField] protected string _fireEffectName;

    [Header("Tower Stats")]
    protected TowerBaseStatsSO _stats;
    public TowerBaseStatsSO Stats => _stats;

    //[SerializeField] protected float _range;
    //[SerializeField] protected float _damage = 1f;
    //[SerializeField] protected float _shotDelay = 0.5f;
    //[SerializeField] private int _level = 1;
    //[SerializeField] private int _price = 10;

    //[Header("Tower Info")]
    //[SerializeField] protected string _name = "base tower";
    //[SerializeField] private TowerType _towerType;


    [Header("Buff / Visual Effects")]
    [SerializeField] protected Transform _buffEffectPoint;
    [SerializeField] protected string _buffEffectName;

    private GameObject _currentBuffEffect = null;
    private GameObject _tile = null;


    //프로퍼티들
    //public TowerType TowerType
    //{
    //    get { return _towerType; }
    //    set { _towerType = value; }
    //}
    //public int Price
    //{
    //    get { return _price; }
    //    set { _price = value; }
    //}
    //public int Level
    //{
    //    get { return _level; }
    //    set { _level = value; }
    //}

    public GameObject Tile
    {
        get { return _tile; }
        set { _tile = value; }
    }

    protected float _nextShot = 0;
    protected List<Transform> _target = new List<Transform>(); //범위에 들어온 적 담기위한 리스트
    protected SphereCollider _sphereCollider;
    protected Rigidbody _rb;
    protected float _baseShotDelay; //버프 코루틴이 끝난후 다시 원래 공격속도로 돌아가기위한 필드
    protected Coroutine _buffCoroutine = null;

    protected virtual void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;

        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.useGravity = false;

        SetRange();
        _baseShotDelay = Stats._shotDelay;
        _nextShot = 0f;
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
            Debug.Log("adfas");
            if(Stats ==null)
            {
                Debug.Log("Stats가 널이야~!");
            }
            _sphereCollider.radius = Stats._range;
        }
    }
    /// <summary>
    /// 첫번째 적을 타겟으로 설정하기위한 메서드
    /// 타겟 리스트를 역순으로 돌면서 상대가 죽거나 비활성화된 상태면 리스트에서 제거함
    /// 아직 적이살아있으면 첫번째로 들어온 적을 바라봄
    /// </summary>
    protected virtual void SetTarget()
    {
        for(int i=_target.Count-1;i>=0;i--)
        {
            Transform currentTarget = _target[i];

            if(currentTarget == null || currentTarget.gameObject.activeSelf==false || currentTarget.gameObject.GetComponent<EnemyBase>().IsDead)
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
    /// <summary>
    /// 버프를 이펙트 켜거나 끄는 메서드
    /// </summary>
    /// <param name="isBuffed"></param>
    protected void ToggleBuffEffect(bool isBuffed)
    {
        

        if (isBuffed)
        {
            if(_currentBuffEffect == null)
            {
                

                _currentBuffEffect = EffectManager.Instance.PlayEffect(_buffEffectName,
                                                _buffEffectPoint.position,
                                                _buffEffectPoint.rotation,
                                                transform);
                
            }
        }
        else
        {
            if(_currentBuffEffect != null)
            {
                Destroy(_currentBuffEffect);
                _currentBuffEffect = null;
            }
        }
    }
    /// <summary>
    /// 적을 공격하는 메서드 버츄얼로 선언해 자식들이 재정의 가능하게만듬
    /// 적이 죽거나 비활성화 됬는지 한번더 체크하고
    /// 총알 발사전에 여기서 데미지를 전달해주고 발사함
    /// </summary>
    protected virtual void AttackTarget()
    {
        if(_target.Count == 0 || Time.time < _nextShot)
        {
            return;
        }
        Transform targetEnemy = _target[0];
        if (targetEnemy == null || !targetEnemy.gameObject.activeSelf || targetEnemy.gameObject.GetComponent<EnemyBase>().IsDead)
        {
            _target.RemoveAt(0);
            return;
        }

        //총알 발사용 쿼터니언값
        Vector3 dirToEnemy = (targetEnemy.position - _firePoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dirToEnemy);

        //사운드 클립 재생
        SoundManager.Instance.Clip("Bullet");

        //총알 발사 이펙트 출력
        EffectManager.Instance.PlayEffect(_fireEffectName, _firePoint.position,_firePoint.rotation,transform);


        GameObject _bullet = _bulletManager.MakeBullet(_bulletType);
        
        if (_bullet != null)
        {
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = targetRotation;

            BulletBase _setBulletComponent = _bullet.GetComponent<BulletBase>();
            if(_setBulletComponent != null)
            {
                _setBulletComponent.SetDamage(Stats._damage);
                _setBulletComponent.Shoot();
            }
            _nextShot = Time.time + Stats._shotDelay;
        }
    }
    /// <summary>
    /// 코루틴 시작전에 한번초기화 시켜주고 코루틴실행
    /// </summary>
    /// <param name="amount">버프효과</param>
    /// <param name="duration">지속시간</param>
    public void ApplyAttackSpeedBuff(float amount,float duration)
    {
        if(_buffCoroutine != null)
        {
            StopCoroutine(_buffCoroutine);
            Stats._shotDelay = _baseShotDelay;
        }
        _buffCoroutine = StartCoroutine(AttackSpeedBuffCoroutine(amount, duration));
    }
    /// <summary>
    /// 버프효과만큼 샷딜레이를 줄여서 공격속도 상승시킴
    /// 지속시간만큼 지속되고 다시 원래공격속도로 돌아감
    /// </summary>
    /// <param name="amount">버프효과</param>
    /// <param name="duration">지속시간</param>
    /// <returns></returns>
    private IEnumerator AttackSpeedBuffCoroutine(float amount,float duration)
    {
        ToggleBuffEffect(true);

        Stats._shotDelay *= (1f - amount);

        yield return new WaitForSeconds(duration);

        Stats._shotDelay = _baseShotDelay;
        _buffCoroutine = null;
        ToggleBuffEffect(false);
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

