using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전역 오브젝트 풀 매니저 (SingleTon 기반)
/// 사용 예시:
/// 1. 스폰
/// var enemy = PoolService.Instance.Get(_enemyPrefab);
/// enemy.Init(...);
///
/// 2. 반납
/// PoolService.Instance.Return(enemy, _enemyPrefab);
///
/// 3. 미리 생성(예열)
/// PoolService.Instance.GetPool(_enemyPrefab, preload: 10);
/// </summary>
public class PoolService : SingleTon<PoolService>
{
    // 프리팹을 키로 풀 객체를 보관하는 딕셔너리
    private readonly Dictionary<Component, object> _poolMap = new Dictionary<Component, object>();

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 특정 프리팹에 대한 풀을 가져오거나 새로 생성
    /// </summary>
    public ObjectPool<T> GetPool<T>(T prefab, int preload = 0) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError("[PoolService] Prefab is null");
            return null;
        }

        // 이미 등록된 풀 있으면 반환
        if (_poolMap.TryGetValue(prefab, out var poolObj))
        {
            return (ObjectPool<T>)poolObj;
        }

        // 새 풀 생성
        var root = GetOrCreateRoot(typeof(T).Name);
        var pool = new ObjectPool<T>(prefab, preload, root);
        _poolMap.Add(prefab, pool);

        return pool;
    }

    /// <summary>
    /// 프리팹으로부터 풀에서 오브젝트 꺼내기
    /// </summary>
    public T Get<T>(T prefab) where T : Component
    {
        var pool = GetPool(prefab);
        return pool != null ? pool.Get() : null;
    }

    /// <summary>
    /// 사용이 끝난 오브젝트를 풀로 반납
    /// </summary>
    public void Return<T>(T instance, T prefab) where T : Component
    {
        var pool = GetPool(prefab);
        if (pool == null)
        {
            Debug.LogWarning("[PoolService] Pool not found. Destroy fallback.");
            Object.Destroy(instance.gameObject);
            return;
        }

        pool.Return(instance);
    }

    /// <summary>
    /// 풀 오브젝트 정리용 부모 Transform 확보
    /// Hierarchy 깔끔하게 정리됨
    /// </summary>
    private Transform GetOrCreateRoot(string typeName)
    {
        var go = GameObject.Find($"__Pool__{typeName}");
        if (go == null)
        {
            go = new GameObject($"__Pool__{typeName}");
            DontDestroyOnLoad(go);
        }

        return go.transform;
    }
}