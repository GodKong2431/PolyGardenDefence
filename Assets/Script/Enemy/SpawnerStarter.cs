using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnerStarter : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO _enemyStats;
    private void Start()
    {
        EnemySpawner.Instance.Spawn(_enemyStats, count: 5, pathId: 0, interval: 0.8f);
    }
}
