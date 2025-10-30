using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private ScenePoolService _pool;   // 인스펙터로 같은 씬의 ScenePoolService 할당 
    [SerializeField] private List<EnemyBase> _enemyPrefabs = new();
    [SerializeField] private bool _usePartialNameMatch = true;

    public void Spawn(EnemyStatsSO stats, int count, int pathId, float interval)
    {
        if (stats == null) { Debug.LogError("[EnemySpawner] stats null"); return; }
        StartCoroutine(SpawnRoutine(stats, count, pathId, interval));
    }

    private IEnumerator SpawnRoutine(EnemyStatsSO stats, int count, int pathId, float interval)
    {
        var path = MapManager.Instance.GetPath(pathId);
        if (path == null) yield break;

        var prefab = FindPrefabFor(stats);
        if (prefab == null) yield break;

        // (선택) 예열
        _pool.GetPool(prefab, preload: count);

        for (int i = 0; i < count; i++)
        {
            var enemy = _pool.Get(prefab);           // 씬 풀에서 꺼냄(재사용)
            enemy.transform.position = path.Points[0].position;
            enemy.SetPrefabRef(prefab);
            enemy.SetPoolService(_pool);             // 반환 대상 풀 지정
            enemy.Init(stats, path);
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// StatsSO에 맞는 프리팹을 리스트에서 찾아 반환.
    /// 기본은 이름 기반 매칭(정확/부분), 실패 시 첫 항목 폴백.
    /// </summary>
    private EnemyBase FindPrefabFor(EnemyStatsSO stats)
    {
        if (_enemyPrefabs == null || _enemyPrefabs.Count == 0) return null;

        // 1) 정확히 같은 이름
        var exact = _enemyPrefabs.Find(p => p != null && p.name == stats.name);
        if (exact != null) return exact;

        // 2) 부분 일치(옵션)
        if (_usePartialNameMatch)
        {
            var partial = _enemyPrefabs.Find(p => p != null && p.name.Contains(stats.name));
            if (partial != null) return partial;
        }

        // 3) 폴백: 첫 번째 프리팹
        Debug.LogWarning($"[EnemySpawner] Prefab match not found for Stats '{stats.name}'. Fallback to first.");
        return _enemyPrefabs[0];
    }
}