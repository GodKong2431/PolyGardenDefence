using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class EnemyBase : MonoBehaviour, IPoolable,IDamageable
{
    [Header("Stats")]
    [SerializeField] private EnemyStatsSO _stats;             // HP/Speed/AttackDamage/AttackDelay 등

    [Header("Components")]
    [SerializeField] private EnemyMovement _movement;         // 직렬화 우선, 누락 시 자동 보정
    [SerializeField] private Animator _animator;              // Attack/Die 트리거 사용(없어도 동작)
    [SerializeField] private EnemyHpBar _enemyHpBar;          // 몬스터 HP바 컴포넌트

    [Header("Attack Detect")]
    [SerializeField] private string _playerTag = "Player";    // 플레이어 감지용 태그
    [SerializeField] private float _attackAnimLead = 0.0f;    // 예시: 모션 선딜(초) (현재 코드에는 없음, 0이면 즉시)

    [Header("Pooling")]
    [SerializeField] private EnemyBase _prefabRef;           // 어느 프리팹 풀인지
    private PoolService _poolService;                   // 어느 풀 매니저로 돌려보낼지

    [Header("Dead")]
    [SerializeField] private bool _isDead;
    public bool IsDead => _isDead;
    public void SetPrefabRef(EnemyBase prefab) => _prefabRef = prefab;
    public void SetPoolService(PoolService svc) => _poolService = svc;

    private float _currentHp;
    private Transform _target;                                // 공격 대상(플레이어)
    private Coroutine _attackCo;
    private Collider _col;                                    // 공격 범위 감지용(Trigger)


    public EnemyStatsSO Stats => _stats;
    // -------- 라이프사이클 --------
    private void Awake()
    {
        if (_movement == null)
        {
            _movement = GetComponent<EnemyMovement>(); // 자동 보정
        }

        _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.isTrigger = true; // 감지용 트리거 보장
        }
    }

    private void OnEnable()
    {
        ResetState();
    }

    private void OnDisable()
    {
        StopAttack();
    }

    private void ResetState()
    {
        if (_stats != null)
        {
            _currentHp = _stats.hp;
        }

        _target = null;
        StopAttack();

        // 애니 초기화(있을 때만)
        if (_animator != null)
        {
            _animator.Rebind();
            _animator.Update(0f);
        }
    }

    // -------- 외부 초기화(스폰 시 호출) --------
    public void Init(EnemyStatsSO stats, WaypointPath path)
    {
        if (stats == null || path == null)
        {
            Debug.LogError("[EnemyBase] Init failed: stats or path is null");
            return;
        }

        _stats = stats;
        _currentHp = _stats.hp;

        if (_movement == null)
        {
            _movement = GetComponent<EnemyMovement>();
        }

        _movement.Init(path, _stats.speed);

        if (_animator != null)
        {
            _animator.Rebind();
            _animator.Update(0f);
        }
    }

    public void HpBarInit(EnemyHpBar enemyHpBar)
    {
        _enemyHpBar = enemyHpBar;
        _enemyHpBar.SetTarget(gameObject.transform);
    }


    // -------- 피해/사망 --------
    public void ApplyDamage(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }
        if (_isDead) 
        {
            return; 
        }  // ← 이미 사망 상태면 무시

        _currentHp -= amount;
        if (_enemyHpBar != null)
        {
            _enemyHpBar.SetHP(_currentHp / _stats.hp);
        }
        if (_currentHp > 0f)
        {
            return;
        }
        Die();
    }

    private void Die()
    {
        var enemy = GetComponent<EnemyBase>();
        _isDead = true;            // ← 사망 플래그 on
        StopAttack();

        // 타워 탐지 차단: 콜라이더 끄기
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (col != null) 
            { col.enabled = false; }
        }

        if (_animator != null)
        {
            _movement?.Stop();
            _animator.SetTrigger("Die");
            StartCoroutine(DespawnAfter(3.0f)); // 예시: 사망 모션 후 1초 뒤 반환
            GameManager.Instance.OnEnemyKilled(enemy.Stats.bounty);
            return;
        }

        Despawn();
    }

    private IEnumerator DespawnAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Despawn();
    }

    public void Despawn()
    {
        _movement?.Stop();

        if (_enemyHpBar != null)
        {
            _enemyHpBar.Despawn();
            _enemyHpBar = null;
        }


        if (_poolService == null || _prefabRef == null)
        {
            Debug.LogWarning("[EnemyBase] poolService/prefabRef null. Destroy fallback.");
            Destroy(gameObject);
            return;
        }

        _poolService.Return(this, _prefabRef);  // 전역이 아니라 "씬 풀"로 반환
    }

    // -------- 공격 감지/처리 --------
    private void OnTriggerEnter(Collider other)
    {
        if (_target != null) return; // 이미 타겟이 있으면 무시

        if (other.CompareTag(_playerTag))
        {
            _target = other.transform;

            _movement?.Stop();

            if (_attackCo == null)
            {
                _attackCo = StartCoroutine(AttackRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_target != null && other.transform == _target)
        {
            StopAttack(true);
            _movement?.Resume(); // 목표 이탈 시 이동 재개
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (_target != null)
        {
            // 회전(정면 보기)
            var dir = (_target.position - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }

            // 공격 모션 트리거
            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
            }

            if (_attackAnimLead > 0f)
            {
                yield return new WaitForSeconds(_attackAnimLead);
            }

            // 실제 피해 적용
            // 플레이어 쪽에 public void TakeDamage(float)가 있다면 호출됨
            _target.SendMessage("TakeDamage", _stats.attackDamage, SendMessageOptions.DontRequireReceiver);

            // 공격 간격
            yield return new WaitForSeconds(_stats.attackDelay);
        }

        StopAttack(true);
    }

    private void StopAttack(bool resumeMove = false)
    {
        if (_attackCo != null)
        {
            StopCoroutine(_attackCo);
            _attackCo = null;
        }

        _target = null;

        if (resumeMove)
            _movement?.Resume();
    }

    // -------- IPoolable --------
    public void OnGetFromPool()
    {
        StopAllCoroutines();
        ResetState();

        _isDead = false;  // ← 플래그 복구
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (col != null) { col.enabled = true; }
        }
    }



    public void OnReturnToPool()
    {
        StopAllCoroutines();
        _movement?.Stop();
        if (_animator != null)
        {
            _animator.ResetTrigger("Attack");
            _animator.ResetTrigger("Die");
        }
    }

    private Coroutine _slowCo; // 중첩 방지를 위한 코루틴 참조

    /// <summary>
    /// 적에게 슬로우 효과 적용
    /// </summary>
    /// <param name="slowRate">감속 비율 (예: 0.5f → 50% 속도)</param>
    /// <param name="duration">지속 시간 (초)</param>
    public void GetSlow(float slowRate, float duration)
    {
        if (_slowCo != null)
        {
            StopCoroutine(_slowCo);
        }
        _slowCo = StartCoroutine(SlowRoutine(slowRate, duration));
    }

    private IEnumerator SlowRoutine(float slowRate, float duration)
    {
        if (_movement == null)
            yield break;

        // 원래 속도 저장
        float originalSpeed = _stats.speed;

        // 감속 적용
        _movement.SetSpeed(originalSpeed * slowRate);

        // 일정 시간 대기
        yield return new WaitForSeconds(duration);

        // 원래 속도로 복원
        _movement.SetSpeed(originalSpeed);

        _slowCo = null;
    }
}