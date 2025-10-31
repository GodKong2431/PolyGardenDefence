using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolService : MonoBehaviour
{
    // 프리팹별 풀
    private readonly Dictionary<Component, object> _poolMap = new();

    public ObjectPool<T> GetPool<T>(T prefab, int preload = 0) where T : Component
    {
        if (prefab == null) { Debug.LogError("[ScenePoolService] Prefab is null"); return null; }
        if (_poolMap.TryGetValue(prefab, out var poolObj))
            return (ObjectPool<T>)poolObj;

        // 씬 전용 루트(씬 안에만 존재)
        var rootGo = new GameObject($"__ScenePool__{typeof(T).Name}");
        rootGo.transform.SetParent(transform, false); // 씬 안에만 존재
        var pool = new ObjectPool<T>(prefab, preload, rootGo.transform);
        _poolMap.Add(prefab, pool);
        return pool;
    }

    public T Get<T>(T prefab) where T : Component
    {
        var pool = GetPool(prefab);
        return pool != null ? pool.Get() : null;
    }

    public void Return<T>(T instance, T prefab) where T : Component
    {
        var pool = GetPool(prefab);
        if (pool == null)
        {
            Debug.LogWarning("[ScenePoolService] Pool not found. Destroy fallback.");
            if (instance) Destroy(instance.gameObject);
            return;
        }
        pool.Return(instance);
    }

    // 필요 시: 현재 씬의 풀 모두 비우기(리트라이/리셋 버튼용)
    public void ClearAll()
    {
        foreach (var kv in _poolMap)
        {
            if (kv.Value is System.IDisposable d) d.Dispose(); // 선택: 확장 시
            var typeName = kv.Key ? kv.Key.GetType().Name : "Unknown";
            var root = transform.Find($"__ScenePool__{typeName}");
            if (root) Destroy(root.gameObject);
        }
        _poolMap.Clear();
    }
}