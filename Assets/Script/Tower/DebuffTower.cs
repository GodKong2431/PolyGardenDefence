using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : TowerBase
{
    [Header("DeBuffTowerSet")]
    [SerializeField] float _slowAmount = 0.5f; //이동속도 감소 비율
    [SerializeField] float _slowDuration = 2f; //슬로우 지속시간
    [SerializeField] Transform _DeBuffEffectPoint;
    [SerializeField] string _DeBuffEffectName;
    protected override void Awake()
    {
        _damage = 0f;
        base.Awake();
    }
    protected override void Update()
    {
        SetTarget();

        if(_target.Count == 0 || Time.time < _nextShot)
        {
            return;
        }
        GiveDebuffs();
        _nextShot = Time.time + _shotDelay;
    }

    private void GiveDebuffs()
    {
        EffectManager.Instance.PlayEffect(_DeBuffEffectName, _DeBuffEffectPoint.position, _DeBuffEffectPoint.rotation, transform);
        SoundManager.Instance.Clip("magic");

        for (int i=_target.Count-1; i>=0; i--)
        {
            Transform _enemyTransform = _target[i];

            EnemyBase _enemy = _enemyTransform.GetComponent<EnemyBase>();

            if(_enemy != null)
            {
                _enemy.GetSlow(_slowAmount, _slowDuration);
            }
        }
    }
}
