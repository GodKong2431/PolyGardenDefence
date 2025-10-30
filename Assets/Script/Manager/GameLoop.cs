using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : SingleTon<GameLoop>
{
    private EnemyStatsSO _enemyStats;
    private bool _isloop = false;
    private WaveController _waveController;
    public void StartLoop()
    {
        if (!_isloop)
        {
            StartCoroutine(Loop());
        }
    }

    private IEnumerator Loop()
    {

        GameManager.Instance.Wave(); //GameManager 웨이브 작동
        _isloop = true;
        _waveController.StartCoroutine("RunSequence");//WaveController 작동
        yield return new WaitUntil(IsWave);
        if (GameManager.Instance.CurrentLife <= 0)//라이프 0이되면 코루틴 종료
        {
            yield break; 
        }
        _isloop = false;
    }
    private bool IsWave()
    {
        return EnemyTracker.Instance != null && EnemyTracker.Instance.ActiveEnemyCount == 0;
        //EnemyTracker가 존재하고 활성 적이 0이면 true 반환
    }
}
