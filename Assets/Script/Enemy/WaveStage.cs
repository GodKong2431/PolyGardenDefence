using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveStage
{
    [Header("Entries")]
    [SerializeField] private List<WaveEntry> _entries = new List<WaveEntry>(); // s 붙임(자료구조 규칙)

    [Header("Delay")]
    [SerializeField] private float _delayAfter = 3f; // 다음 스테이지로 넘어가기 전 대기(초)

    public IReadOnlyList<WaveEntry> Entries => _entries;
    public float DelayAfter => _delayAfter;
}