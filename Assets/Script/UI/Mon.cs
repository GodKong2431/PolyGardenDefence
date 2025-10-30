using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon : MonoBehaviour
{

    public Transform HeadTransform; // 머리 위치 기준점 (빈 오브젝트를 자식으로 두면 좋아요)
    private MonsterHpBar _hpBar;

    private void Awake()
    {
        
    }

    public void Init(MonsterHpBar hpBar)
    {
        _hpBar = hpBar;
        _hpBar.SetTarget(HeadTransform); // HP바가 따라다닐 대상을 연결
    }

    public void TakeDamage(float damage)
    {
        // HP 감소 로직 ...
        float hpRatio = 0.5f; // 예시
        _hpBar.SetHP(hpRatio);
    }

}
