using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private UpgradeUI _upgradeUI;

    private int _layerCount;
    private Camera _camera;
    

    //싱글톤 Awake도 발동하고 초기화
    protected  void Awake()
    {
        Init();
    }

    //업데이트마다 Ray 발사
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryObjectSelect();
        }
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, _layerCount);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Tower"))
            {
                Debug.Log(hit.transform.name);

                _upgradeUI.MoveToTower(hit.transform.gameObject);
                return;
            }
        }
    }

    //태그 비교 메서드
    private void InteractionObjectCompareTag(RaycastHit hit)
    {
        switch (hit.transform.gameObject.tag)
        {
            case "Tower":
                
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
