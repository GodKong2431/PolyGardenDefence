using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 개별 프리팹 단위의 제네릭 오브젝트 풀.
/// PoolService가 이 클래스를 여러 개 만들어 관리함.
/// </summary>
/// <typeparam name="T">Component 타입 (예: EnemyBase, Projectile 등)</typeparam>
public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> _q = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _root;

    /// <summary>
    /// 풀 생성자
    /// </summary>
    /// <param name="prefab">풀링할 원본 프리팹</param>
    /// <param name="preload">미리 생성할 수량</param>
    /// <param name="root">Hierarchy 정리를 위한 부모 Transform</param>
    public ObjectPool(T prefab, int preload, Transform root)
    {
        _prefab = prefab;
        _root = root;

        for (int i = 0; i < preload; i++)
        {
            var obj = Object.Instantiate(_prefab, _root);
            obj.gameObject.SetActive(false);
            _q.Enqueue(obj);
        }
    }

    /// <summary>
    /// 풀에서 하나 꺼냄 (없으면 새로 생성)
    /// </summary>
    public T Get()
    {
        var obj = _q.Count > 0 ? _q.Dequeue() : Object.Instantiate(_prefab, _root);
        obj.gameObject.SetActive(true);

        if (obj is IPoolable p)
        {
            p.OnGetFromPool();
        }

        return obj;
    }

    /// <summary>
    /// 풀에 오브젝트를 반납
    /// </summary>
    public void Return(T obj)
    {
        if (obj is IPoolable p)
        {
            p.OnReturnToPool();
        }

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_root, false);
        _q.Enqueue(obj);
    }

    /// <summary>
    /// 필요한 만큼 미리 생성해둘 때 사용 (게임 시작 시 예열)
    /// </summary>
    public void Warm(int count)
    {
        while (_q.Count < count)
        {
            var obj = Object.Instantiate(_prefab, _root);
            obj.gameObject.SetActive(false);
            _q.Enqueue(obj);
        }
    }
}