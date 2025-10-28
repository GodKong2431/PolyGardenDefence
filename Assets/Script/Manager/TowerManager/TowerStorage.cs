using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStorage : SingleTon<TowerStorage>
{
    public Dictionary<TowerType, GameObject> BasicTowers {  get; private set; }
    public Dictionary<TowerType, GameObject> AdvancedTowers { get; private set; }
    public Dictionary<TowerType, GameObject> FinalTowers { get; private set; }

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
        BasicTowers = new Dictionary<TowerType, GameObject>();
        AdvancedTowers = new Dictionary<TowerType, GameObject>();
        FinalTowers = new Dictionary<TowerType, GameObject>();

        BasicTowers.Add(TowerType.Ballista, _ballista_0);
        BasicTowers.Add(TowerType.Catapult, _catapult_0);
        BasicTowers.Add(TowerType.Cannon, _cannon_0);
        BasicTowers.Add(TowerType.Turret, _turret_0);
        BasicTowers.Add(TowerType.Buff, _buffTower_0);
        BasicTowers.Add(TowerType.Debuff, _debuffTower_0);

        AdvancedTowers.Add(TowerType.Ballista, _ballista_1);
        AdvancedTowers.Add(TowerType.Catapult, _catapult_1);
        AdvancedTowers.Add(TowerType.Cannon, _cannon_1);
        AdvancedTowers.Add(TowerType.Turret, _turret_1);
        AdvancedTowers.Add(TowerType.Buff, _buffTower_1);
        AdvancedTowers.Add(TowerType.Debuff, _debuffTower_1);

        FinalTowers.Add(TowerType.Ballista, _ballista_2);
        FinalTowers.Add(TowerType.Catapult, _catapult_2);
        FinalTowers.Add(TowerType.Cannon, _cannon_2);
        FinalTowers.Add(TowerType.Turret, _turret_2);
        FinalTowers.Add(TowerType.Buff, _buffTower_2);
        FinalTowers.Add(TowerType.Debuff, _debuffTower_2);
    }

}
