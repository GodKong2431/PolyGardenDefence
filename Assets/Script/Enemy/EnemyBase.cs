using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IPoolable
{
    [Header("Stats")]
    [SerializeField] private EnemyStatsSO _stats;

    [Header("Components")]
    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private Animator _anim;

    private float _hp;

    // 풀 반환 시 참조할 프리팹(Spawner에서 연결해주거나, Factory 주입)
    [SerializeField] private EnemyBase _prefabRef;

    public EnemyStatsSO Stats => _stats;

    public void Init(EnemyStatsSO stats, WaypointPath path)
    {
        if (stats == null || path == null)
        {
            Debug.LogError("[EnemyBase] Init failed: stats or path is null");
            return;
        }

        _stats = stats;
        _hp = _stats.hp;

        if (_movement == null)
        {
            _movement = GetComponent<EnemyMovement>();
        }

        _movement.Init(path, _stats.speed);

        if (_anim != null)
        {
            _anim.Rebind();
            _anim.Update(0f);
        }
    }

    public void ApplyDamage(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        _hp -= amount;

        if (_hp > 0f)
        {
            return;
        }

        Die();
    }

    private void Die()
    {
        GameManager_Demo.Instance.OnEnemyKilled(_stats.bounty);

        if (_anim != null)
        {
            _movement.Stop();
            _anim.SetTrigger("Die");
            StartCoroutine(DespawnAfter(1.0f));
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
        _movement.Stop();

        if (_prefabRef == null)
        {
            Debug.LogWarning("[EnemyBase] prefabRef is null. Destroy fallback.");
            Destroy(gameObject);
            return;
        }

        PoolService.Instance.Return(this, _prefabRef);
    }

    // IPoolable 훅
    public void OnGetFromPool() { /* 상태/이펙트 초기화 필요 시 */ }
    public void OnReturnToPool()
    {
        StopAllCoroutines();
        if (_anim != null) _anim.ResetTrigger("Die");
    }

    // 선택) 프리팹 참조 주입용
    public void SetPrefabRef(EnemyBase prefab) => _prefabRef = prefab;
}