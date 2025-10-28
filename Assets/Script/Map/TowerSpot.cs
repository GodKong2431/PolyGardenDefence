using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))] // 클릭 레이캐스트용 
public class TowerSpot : MonoBehaviour
{
    [SerializeField] private bool _isOccupied = false;   // 현재 타워 설치 여부
    [SerializeField] private Vector3 _placeOffset = Vector3.zero; // 배치 위치 조정용(선택)

    public bool IsOccupied => _isOccupied;

    /// <summary>
    /// 현재 이 스팟이 비어 있는지 (설치 가능 여부)
    /// </summary>
    public bool CanPlace()
    {
        return !_isOccupied;
    }

    /// <summary>
    /// 타워를 설치했을 때 호출
    /// </summary>
    public void Occupy()
    {
        _isOccupied = true;
    }

    /// <summary>
    /// 타워 제거 시 호출
    /// </summary>
    public void Clear()
    {
        _isOccupied = false;
    }

    /// <summary>
    /// 타워가 놓일 실제 위치 반환
    /// </summary>
    public Vector3 GetPlacePosition()
    {
        return transform.position + _placeOffset;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _isOccupied ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.1f, Vector3.one);
    }
#endif
}