using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHero_Demo : MonoBehaviour
{
    [SerializeField] private float _hp = 100f;

    public void TakeDamage(float dmg)
    {
        _hp -= dmg;
        if (_hp <= 0f)
        {
            Debug.Log("플레이어 사망");
            // 사망 처리 로직 (게임오버 등)
        }
    }
}