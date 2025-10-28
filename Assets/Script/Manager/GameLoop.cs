using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : SingleTon<GameLoop>
{
    private EnemyStatsSO _enemyStats;
    private bool _isloop = false;
    public void StartLoop()
    {
        if (!_isloop)
        {
            StartCoroutine(Loop());
        }
        //버튼 누르면 시작
    }

    private IEnumerator Loop()
    {

        GameManager.Instance.Wave(); //GameManager 웨이브 작동
        _isloop = true;
        EnemySpawner.Instance.Spawn(_enemyStats, count: 5, pathId: 0, interval: 0.8f);
        //EnemySpawner의 Spawn작동()안에 있는것은 예시
        yield return new WaitUntil(IsWave);
        _isloop = false;
    }
    private bool IsWave()
    {
        return EnemyTracker.Instance != null && EnemyTracker.Instance.ActiveEnemyCount == 0 || GameManager.Instance.CurrentLife <= 0;
        //EnemyTracker가 존재하고 활성 적이 0이면 true 반환 life 0이되면 반환
    }
}
