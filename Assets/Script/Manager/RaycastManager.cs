using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] private LayerMask _layerMask;

    private Camera _camera;
    private Ray _ray;
    RaycastHit hit;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        TryObjectSelect();
    }

    private void Init()
    {
        _camera = Camera.main;
    }

    private void TryObjectSelect()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out hit, 1000, _layerMask))
        {
            Debug.Log(hit.transform.name);
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform == null)
                    return;

                Debug.Log("Å¬¸¯");
            }
        }
    }
}
