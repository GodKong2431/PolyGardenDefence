using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStorage : SingleTon<TowerStorage>
{
    public Dictionary<TowerType, GameObject> _basicTowers;
    public Dictionary<TowerType, GameObject> _advancedTowers;
    public Dictionary<TowerType, GameObject> _finalTowers;

    [SerializeField] GameObject _ballista_0;
    [SerializeField] GameObject _ballista_1;
    [SerializeField] GameObject _ballista_2;
    [SerializeField] GameObject _catapult_0;
    [SerializeField] GameObject _catapult_1;
    [SerializeField] GameObject _catapult_2;
    [SerializeField] GameObject _cannon_0;
    [SerializeField] GameObject _cannon_1;
    [SerializeField] GameObject _cannon_2;
    [SerializeField] GameObject _turret_0;
    [SerializeField] GameObject _turret_1;
    [SerializeField] GameObject _turret_2;
    [SerializeField] GameObject _buffTower_0;
    [SerializeField] GameObject _buffTower_1;
    [SerializeField] GameObject _buffTower_2;
    [SerializeField] GameObject _debuffTower_0;
    [SerializeField] GameObject _debuffTower_1;
    [SerializeField] GameObject _debuffTower_2;

    protected override void Awake()
    {
        base.Awake();
        SetStorage();
    }

    private void SetStorage()
    {
        _basicTowers = new Dictionary<TowerType, GameObject>();
        _advancedTowers = new Dictionary<TowerType, GameObject>();
        _finalTowers = new Dictionary<TowerType, GameObject>();

        _basicTowers.Add(TowerType.Ballista, _ballista_0);
        _basicTowers.Add(TowerType.Catapult, _catapult_0);
        _basicTowers.Add(TowerType.Cannon, _cannon_0);
        _basicTowers.Add(TowerType.Turret, _turret_0);
        _basicTowers.Add(TowerType.Buff, _buffTower_0);
        _basicTowers.Add(TowerType.Debuff, _debuffTower_0);

        _advancedTowers.Add(TowerType.Ballista, _ballista_1);
        _advancedTowers.Add(TowerType.Catapult, _catapult_1);
        _advancedTowers.Add(TowerType.Cannon, _cannon_1);
        _advancedTowers.Add(TowerType.Turret, _turret_1);
        _advancedTowers.Add(TowerType.Buff, _buffTower_1);
        _advancedTowers.Add(TowerType.Debuff, _debuffTower_1);

        _finalTowers.Add(TowerType.Ballista, _ballista_2);
        _finalTowers.Add(TowerType.Catapult, _catapult_2);
        _finalTowers.Add(TowerType.Cannon, _cannon_2);
        _finalTowers.Add(TowerType.Turret, _turret_2);
        _finalTowers.Add(TowerType.Buff, _buffTower_2);
        _finalTowers.Add(TowerType.Debuff, _debuffTower_2);
    }

}
