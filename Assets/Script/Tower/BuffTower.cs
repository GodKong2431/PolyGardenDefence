using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SphereCollider))]
public class BuffTower : TowerBase
{
    [Header("BuffTowerSet")]
    [SerializeField] float _buffAmount=0.5f; // 버프효과
    [SerializeField] float _buffDuration=2.0f; // 버프 지속시간
    [SerializeField] float _buffDelay = 3.0f; //버프 적용간격
    [SerializeField] Transform _EffectPoint;
    [SerializeField] string _buffTowerEffectName;
    private float _nextBuffTime = 0f;
    private List<TowerBase> _friendlyTower = new List<TowerBase>();
    

    protected override void Awake()
    {
        _damage = 0f;
        _shotDelay = _buffDelay;
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
        if(Time.time >= _nextBuffTime)
        {
            GiveBuffs();
            _nextBuffTime = Time.time + _buffDelay;
        }
    }
    private void GiveBuffs()
    {
            EffectManager.Instance.PlayEffect(_buffTowerEffectName, _EffectPoint.position, _EffectPoint.rotation, transform);
            SoundManager.Instance.Clip("Buff");
        
        for(int i=_friendlyTower.Count-1; i>=0; i--)
        {
            TowerBase friendTower = _friendlyTower[i];

            if(friendTower == null || friendTower.gameObject.activeSelf==false)
            {
                _friendlyTower.RemoveAt(i);
                continue;
            }
            friendTower.ApplyAttackSpeedBuff(_buffAmount, _buffDuration);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            TowerBase tower = other.GetComponent<TowerBase>();

            if (!_friendlyTower.Contains(tower) && tower != null && tower != this)
            {
                _friendlyTower.Add(tower);
                
            }
        }
        
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            TowerBase tower = other.GetComponent<TowerBase>();

            if (_friendlyTower.Contains(tower) && tower != null)
            {
                _friendlyTower.Remove(tower);
                
            }
        }
    }
}
