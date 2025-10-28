using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingleTon<EnemySpawner>
{
    [Header("Prefab")]
    [SerializeField] private EnemyBase _enemyPrefab;

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

        // 필요 시 예열
        PoolService.Instance.GetPool(_enemyPrefab, preload: count);

        for (int i = 0; i < count; i++)
        {
            var enemy = PoolService.Instance.Get(_enemyPrefab);
            enemy.transform.position = path.Points[0].position;
            enemy.Init(stats, path);
            yield return new WaitForSeconds(interval);
        }
    }
}