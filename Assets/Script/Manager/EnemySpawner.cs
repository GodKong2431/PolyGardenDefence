using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingleTon<EnemySpawner>
{
    [Header("Prefabs")]
    [SerializeField] private List<EnemyBase> _enemyPrefabs = new(); // 여러 적 프리팹 등록

    [Header("Matching")]
    [Tooltip("StatsSO 이름과 프리팹 이름의 부분/정확 일치로 매칭합니다. 필요 시 키를 StatsSO에 추가하세요.")]
    [SerializeField] private bool _usePartialNameMatch = true;

    /// <summary>
    /// 웨이브 엔트리용 스폰 API
    /// </summary>
    public void Spawn(EnemyStatsSO stats, int count, int pathId, float interval)
    {
        if (stats == null)
        {
            Debug.LogError("[EnemySpawner] EnemyStatsSO is null");
            return;
        }
        StartCoroutine(SpawnRoutine(stats, count, pathId, interval));
    }

    private IEnumerator SpawnRoutine(EnemyStatsSO stats, int count, int pathId, float interval)
    {
        var path = MapManager.Instance.GetPath(pathId);
        if (path == null)
        {
            Debug.LogError($"[EnemySpawner] Path not found: {pathId}");
            yield break;
        }

        var prefab = FindPrefabFor(stats);
        if (prefab == null)
        {
            Debug.LogError($"[EnemySpawner] No prefab matched for stats: {stats.name}");
            yield break;
        }

        // 필요 시 예열 (프리팹별 풀)
        PoolService.Instance.GetPool(prefab, preload: count);

        for (int i = 0; i < count; i++)
        {
            var enemy = PoolService.Instance.Get(prefab);
            enemy.transform.position = path.Points[0].position;
            enemy.SetPrefabRef(prefab);        // 풀 반환용 레퍼런스 주입
            enemy.Init(stats, path);           // 스탯/경로 주입
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