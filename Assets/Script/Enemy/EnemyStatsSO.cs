using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/EnemyStats", fileName = "EnemyStats_")]
public class EnemyStatsSO : ScriptableObject
{
    public string enemyName;
    public float hp;
    public float speed;
    public int bounty; // Ã³Ä¡ ½Ã È¹µæ °ñµå
    public bool isBoss;

    [Header("Attack")]
    public float attackDamage = 5f;   // ÇÑ ¹ø °ø°Ý ½Ã ÇÇÇØ·®
    public float attackDelay = 1.2f;
}
