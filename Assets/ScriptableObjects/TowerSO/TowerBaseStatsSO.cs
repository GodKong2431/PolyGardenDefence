using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "TD/Tower Stats/TowerBase",               // Create 메뉴 경로
    fileName = "TowerStats_"                  // 새 파일 기본 이름 접두어
)]
public class TowerBaseStatsSO : ScriptableObject
{
    public string _towerName;
    public int _price;
    public float _damage;
    public float _range;
    public float _shotDelay;    
    public int _level;
    public TowerType _towerType;
    public float _criticalChance;
    public float _criticalMultiplier;
    public float _explosionRadius;
    public int _pierceCount;
    [SerializeField] public float _buffAmount = 0.5f; // 버프효과
    [SerializeField] public float _buffDuration = 2.0f; // 버프 지속시간
    [SerializeField] public float _buffDelay = 3.0f; //버프 적용간격    
    [SerializeField] public float _slowAmount = 0.5f; //이동속도 감소 비율
    [SerializeField] public float _slowDuration = 2f; //슬로우 지속시간    
    [SerializeField] public string _DeBuffEffectName;
}
