using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UpgradeUI : MonoBehaviour
{
    private TowerBase _selectTower;
    public TowerBase SelectTower
    {
        get;
    }


    private Camera _camera;

    Vector3 currentSelectCameraPos;
    private void Awake()
    {
        gameObject.SetActive(false);   
        _camera = Camera.main;
    }
    //!(Input.GetAxis("Mouse ScrollWheel") == 0)
    private void Update()
    {
        if (gameObject.activeSelf && (currentSelectCameraPos != _camera.transform.position))
            gameObject.SetActive(false);
    }

    public void MoveToTower(TowerBase selectTower)
    {
        _selectTower = selectTower;
        Vector3 movePos = selectTower.transform.position;
        currentSelectCameraPos = _camera.transform.position;
        transform.position = _camera.WorldToScreenPoint(movePos);
        gameObject.SetActive(true);
    }
}
