using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private EnemyStatsSO _enemyStats;
    private bool _isloop = false;
    private void Start()
    {

        //스폰 포인트에서 스폰
    }
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
        GameManager.Instance.Wave();
        _isloop = true;
        EnemySpawner.Instance.Spawn(_enemyStats, count: 5, pathId: 0, interval: 0.8f);

        yield return null; 
    }
    private void StartWave()
    {

    }
}
