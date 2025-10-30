using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // BulletType별 프리팹 관리용 구조체
    [Serializable]
    public struct BulletPrefabMap
    {
        public BulletType type;
        public BulletBase prefab;   // [modified] GameObject 대신 BulletBase (IPoolable 포함)
        public int preload;         // [added] 초기 예열용
    }

    [Header("Scene-scoped Pool")]
    [SerializeField] private ScenePoolService _pool;            // 기존 ObjectPool 직접 관리 → ScenePoolService 참조로 변경
    [SerializeField] private List<BulletPrefabMap> _prefabs = new(); // Inspector에서 BulletType ↔ 프리팹 연결 리스트

    // 풀 캐시 (타입별 ObjectPool 관리)
    private readonly Dictionary<BulletType, ObjectPool<BulletBase>> _poolCache = new();

    private void Awake()
    {
        // ScenePoolService 자동 탐색 (없으면 수동 드래그 필요)
        if (_pool == null)
        {
            _pool = FindFirstObjectByType<ScenePoolService>();
        }

        // 선택적 예열 (preload)
        foreach (var e in _prefabs)
        {
            if (e.prefab == null || e.preload <= 0) continue;
            var p = _pool.GetPool(e.prefab, e.preload);
            _poolCache[e.type] = p;
        }
    }

    // GameObject를 반환하는 MakeBullet 유지
    public GameObject MakeBullet(BulletType type)
    {
        // 기존 Instantiate → 풀에서 꺼내기 방식으로 변경
        var prefab = FindPrefab(type);
        if (prefab == null)
        {
            Debug.LogWarning($"[BulletManager] Unknown bullet type: {type}");
            return null;
        }

        var pool = GetOrCreatePool(type, prefab);  // 해당 타입의 풀 획득
        var bullet = pool.Get();                   // 풀에서 BulletBase 꺼냄 (IPoolable.OnGetFromPool 호출됨)
        bullet._type = type;
        bullet.gameObject.SetActive(true);         // 안전하게 활성화

        return bullet.gameObject; // 반환 타입은 GameObject 그대로 유지
    }

    // 총알 반환 API (BulletBase → OffBullet()에서 호출 가능)
    public void Return(BulletBase instance)
    {
        if (instance == null) return;

        var prefab = FindPrefab(instance._type);
        if (prefab == null)
        {
            Destroy(instance.gameObject); // 풀 매핑이 없으면 그냥 삭제
            return;
        }

        _pool.Return(instance, prefab); // ScenePoolService가 IPoolable.OnReturnToPool() 호출
    }

    // 내부 유틸 1 - 타입으로 프리팹 찾기
    private BulletBase FindPrefab(BulletType type)
    {
        for (int i = 0; i < _prefabs.Count; i++)
            if (_prefabs[i].type.Equals(type))
                return _prefabs[i].prefab;
        return null;
    }

    // 내부 유틸 2 - 해당 타입의 풀 가져오기/없으면 생성
    private ObjectPool<BulletBase> GetOrCreatePool(BulletType type, BulletBase prefab)
    {
        if (_poolCache.TryGetValue(type, out var p) && p != null)
            return p;

        p = _pool.GetPool(prefab); // ScenePoolService에서 새 풀 생성
        _poolCache[type] = p;
        return p;
    }
}