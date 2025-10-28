using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : SingleTon<RaycastManager>
{
    private int _layerCount;
    private Camera _camera;
    private Ray _ray;
    private RaycastHit hit;

    //싱글톤 Awake도 발동하고 초기화
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    //업데이트마다 Ray 발사
    private void Update()
    {
        TryObjectSelect();
    }

    //초기화 함수
    private void Init()
    {
        _camera = Camera.main;
        _layerCount = LayerMask.GetMask("InteractionObject");
    }

    //Ray발사하고 레이어 에 맞으면 태그 비교
    private void TryObjectSelect()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out hit, 1000, _layerCount))
        {
            
            Debug.Log(hit.transform.name);
            if (Input.GetMouseButtonDown(0))
            {
                InteractionObjectCompareTag();
            }
        }
    }



    //태그 비교 메서드
    private void InteractionObjectCompareTag()
    {
        switch (hit.transform.gameObject.tag)
        {
            case "Tower":
                Debug.Log("타워 메서드 호출");
                return;
            case "NomalTile":
                Debug.Log("바닥 메서드 호출");
                return;
            case "TowerPlacementTile":
                Debug.Log("설치 메서드 호출");
                return;
            case null:
                return;
        }
    }
}
