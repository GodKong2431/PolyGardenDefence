using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private Vector3 _gab = new Vector3(0, 2f, 0);

    private Transform _target;
    private Camera _mainCam;

    void Start()
    {
        _mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (_target == null) return;

        // 머리 위로 위치 이동
        transform.position = _target.position + _gab;

        // 카메라를 바라보게
        transform.forward = _mainCam.transform.forward;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetHP(float ratio)
    {
        _hpBar.fillAmount = ratio;
    }
}
