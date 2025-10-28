using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : SingleTon<EnemySpawner>
{
    [Header("Enemy Prefabs")]
    [SerializeField] private List<EnemyBase> _enemyPrefabs = new List<EnemyBase>(); // 여러 프리팹 등록

    /// <summary>
    /// EnemyStatsSO에서 적 이름을 받아 해당 프리팹을 찾아 스폰
    /// </summary>
    public void Spawn(EnemyStatsSO stats, int count, int pathId, float interval)
    {
        if (stats == null)
        {
            Debug.LogError("[EnemySpawner] stats is null");
            return;
        }

        // 이름으로 프리팹 검색 (EnemyStatsSO.enemyName과 일치)
        var prefab = _enemyPrefabs.Find(p => p.name == stats.enemyName);
        if (prefab == null)
        {
            Debug.LogWarning($"[EnemySpawner] No prefab found for {stats.enemyName}");
            return;
        }

        StartCoroutine(SpawnRoutine(prefab, stats, count, pathId, interval));
    }

    private IEnumerator SpawnRoutine(EnemyBase prefab, EnemyStatsSO stats, int count, int pathId, float interval)
    {
        var path = MapManager.Instance.GetPath(pathId);
        if (path == null)
        {
            Debug.LogError($"[EnemySpawner] Path not found: {pathId}");
            yield break;
        }

        // 프리팹별 풀 예열
        PoolService.Instance.GetPool(prefab, preload: count);

        for (int i = 0; i < count; i++)
        {
            var enemy = PoolService.Instance.Get(prefab);
            enemy.transform.position = path.Points[0].position;

            enemy.SetPrefabRef(prefab);
            enemy.Init(stats, path);

            if (interval > 0f)
            {
                yield return new WaitForSeconds(interval);
            }
        }
    }
}