using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이 목표 지점에 도달했을 때 감지하는 트리거.
/// - 골인지점에 Collider(Trigger) 부착 후, 이 스크립트를 연결.
/// - EnemyBase와 충돌 시 GameManager에 골인 알림.
/// </summary>
[RequireComponent(typeof(Collider))]
public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 EnemyBase인지 확인
        var enemy = other.GetComponent<EnemyBase>();
        if (enemy == null)
        {
            return;
        }

        // 보스면 즉시 게임 오버, 아니면 라이프 감소
        if (enemy.Stats.isBoss == true)
        {
            GameManager.Instance.Life(false, enemy.Stats.isBoss);
        }
        else
        {
            GameManager.Instance.Life(true, false);
        }

        // 적 비활성화 (오브젝트 풀 반납)
        enemy.Despawn();
        
    }
}