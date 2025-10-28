using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaypointPath : MonoBehaviour
{
    [Header("Path Id")]
    [SerializeField] private int _id;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> _points = new List<Transform>();

    public int Id => _id;
    public IReadOnlyList<Transform> Points => _points;

    private void OnEnable()
    {
        if (MapManager.Instance == null)
        {
            return;
        }

        MapManager.Instance.RegisterPath(this);
    }



    [ContextMenu("Collect Points From Children (Order)")]
    private void CollectPointsFromChildren()
    {
        _points.Clear();
        foreach (Transform child in transform)
            _points.Add(child);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_points == null || _points.Count == 0) return;
        for (int i = 0; i < _points.Count; i++)
        {
            var p = _points[i];
            if (p == null) continue;
            Gizmos.DrawSphere(p.position, 0.2f);
            if (i + 1 < _points.Count && _points[i + 1] != null)
                Gizmos.DrawLine(p.position, _points[i + 1].position);
        }
    }
#endif
}
