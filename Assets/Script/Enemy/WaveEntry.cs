using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEntry
{
    [Header("Enemy")]
    [SerializeField] private EnemyStatsSO _stats;     // 스탯 SO

    [Header("Spawn")]
    [SerializeField] private int _count = 5;          // 마릿수
    [SerializeField] private float _interval = 0.3f;  // 개체 간 스폰 간격
    [SerializeField] private int _pathId = 0;         // MapManager 경로 ID

    public EnemyStatsSO Stats => _stats;
    public int Count => _count;
    public float Interval => _interval;
    public int PathId => _pathId;
}