using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : SingleTon<EnemyTracker>
{
    private List<EnemyBase> _activeEnemies = new List<EnemyBase>();
    public int ActiveEnemyCount => _activeEnemies.Count;

    //적 소환시 등록
    public void Register(EnemyBase enemy)
    {
        if (!_activeEnemies.Contains(enemy))
            _activeEnemies.Add(enemy);
    }

    //적 사망시 등록해제
    public void Unregister(EnemyBase enemy)
    {
        if (_activeEnemies.Contains(enemy))
            _activeEnemies.Remove(enemy);
    }
}
