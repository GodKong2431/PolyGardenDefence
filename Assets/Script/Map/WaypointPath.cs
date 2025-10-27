using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
