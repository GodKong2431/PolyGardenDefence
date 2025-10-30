using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "TD/Tower Stats",               // Create 메뉴 경로
    fileName = "TowerStats_"                  // 새 파일 기본 이름 접두어
)]
public class TowerStatsSO : ScriptableObject
{
    public string towerName;
    public int cost;
    public float damage;
    public float range;
    public float fireRate;
}
