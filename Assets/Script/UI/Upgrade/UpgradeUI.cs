using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private TowerBase _selectTower;
    public TowerBase SelectTower
    {
        get { return _selectTower; }
    }


    private Camera _camera;

    Vector3 currentSelectCameraPos;
    Vector3 currentSelectCameraRot;
    private void Awake()
    {
        gameObject.SetActive(false);   
        _camera = Camera.main;
    }
    //!(Input.GetAxis("Mouse ScrollWheel") == 0)
    private void Update()
    {
        if (gameObject.activeSelf && ((currentSelectCameraPos != _camera.transform.position) || (currentSelectCameraRot != _camera.transform.eulerAngles)))
            gameObject.SetActive(false);
    }

    public void MoveToTower(GameObject selectTower)
    {
        _selectTower = selectTower.GetComponentInChildren<TowerBase>();
        Vector3 movePos = selectTower.transform.position;
        currentSelectCameraPos = _camera.transform.position;
        currentSelectCameraRot = _camera.transform.eulerAngles;
        transform.position = _camera.WorldToScreenPoint(movePos);
        gameObject.SetActive(true);
    }

    public void OnClickAnyButton()
    {
        gameObject.SetActive(false);
    }
}
