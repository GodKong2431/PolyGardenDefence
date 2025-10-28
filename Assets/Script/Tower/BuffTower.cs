using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SphereCollider))]
public class BuffTower : TowerBase
{
    [SerializeField] float _buffAmount;
    [SerializeField] float _buffDuration;

    private List<TowerBase> _friendlyTower = new List<TowerBase>();
    private SphereCollider _collider;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            TowerBase tower = other.GetComponent<TowerBase>();

            if (!_friendlyTower.Contains(tower))
            {
                _friendlyTower.Add(tower);
                //tower.ApplyBuff(_buffAmount, _buffDuration);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            TowerBase tower = other.GetComponent<TowerBase>();

            if (_friendlyTower.Contains(tower))
            {
                _friendlyTower.Remove(tower);
                
            }
        }
    }
}
