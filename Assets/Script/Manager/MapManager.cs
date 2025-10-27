using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingleTon<MapManager>
{
    [Header("Paths")]
    [SerializeField] private List<WaypointPath> _paths = new List<WaypointPath>(); // 인스펙터에 등록 or 런타임 등록
    private Dictionary<int, WaypointPath> _pathMap = new Dictionary<int, WaypointPath>();

    protected override void Awake()
    {
        base.Awake();
        BuildPathMap();
    }

    private void BuildPathMap()
    {
        _pathMap.Clear();
        foreach (var path in _paths)
        {
            if (path == null)
            {
                continue;
            }

            int key = path.Id;
            if (_pathMap.ContainsKey(key))
            {
                // default: 중복 경로ID 발견
                Debug.LogWarning($"[MapManager] Duplicate Path Id: {key}");
                continue;
            }

            _pathMap.Add(key, path);
        }
    }

    public void RegisterPath(WaypointPath path)
    {
        if (path == null)
        {
            return;
        }

        if (_pathMap.ContainsKey(path.Id))
        {
            return;
        }

        _pathMap.Add(path.Id, path);
    }

    public WaypointPath GetPath(int id)
    {
        if (_pathMap.TryGetValue(id, out var path))
        {
            return path;
        }

        Debug.LogWarning($"[MapManager] Path not found: {id}");
        return null;
    }
}
