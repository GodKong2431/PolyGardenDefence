using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Wave Sequence", fileName = "WaveSequence_")]
public class WaveSequenceSO : ScriptableObject
{
    [Header("Stages")]
    [SerializeField] private List<WaveStage> _stages = new List<WaveStage>();

    public IReadOnlyList<WaveStage> Stages => _stages;
}