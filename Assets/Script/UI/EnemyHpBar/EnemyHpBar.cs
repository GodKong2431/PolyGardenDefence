using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour, IPoolable
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Vector3 _gab = new Vector3(0, 2f, 0);

    private Transform _target;
    private Camera _camera;


    private PoolService _pool;
    private EnemyHpBar _prefab;
    private Transform _canvasRoot;


    public void BindPool(PoolService pool, EnemyHpBar prefab, Transform canvasRoot)
    {
        _pool = pool;
        _prefab = prefab;
        _canvasRoot = canvasRoot;
    }



    void Start()
    {
        _camera = Camera.main;
    }

    void LateUpdate()
    {
        if (_target == null) return;

        // 머리 위로 위치 이동
        transform.position = _target.position + _gab;

        // 카메라를 바라보게
        transform.forward = _camera.transform.forward;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetHP(float ratio)
    {
        _hpBar.fillAmount = ratio;
    }

    public void OnGetFromPool()
    {
        if (_canvasRoot) transform.SetParent(_canvasRoot, false);
        gameObject.SetActive(true);

        if (_camera == null) _camera = Camera.main;  // 카메라 캐시 보정
        _hpBar.fillAmount = 1f;                      // 체력바 초기화
        gameObject.SetActive(true);                  // 활성화
    }


    public void OnReturnToPool()
    {
        _target = null;                              // 끊어주지 않으면 Destroy된 타겟 참조 위험
        gameObject.SetActive(false);                 // 비활성화 (풀 큐로 복귀)
        transform.SetParent(null, false); // 풀 루트로 이동은 Pool이 처리
    }

    public void Despawn()
    {
        if (_pool != null && _prefab != null) _pool.Return(this, _prefab);
        else gameObject.SetActive(false); // 폴백(하지만 재사용은 안 됨)
    }
}
