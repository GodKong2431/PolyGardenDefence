using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UpgradeUI : MonoBehaviour
{
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
        if (gameObject.activeSelf && (Input.anyKeyDown || currentSelectCameraPos != _camera.transform.position))
            gameObject.SetActive(false);
    }

    public void MoveToTower(GameObject _selectTower)
    {
        Vector3 movePos = _selectTower.transform.position;
        currentSelectCameraPos = _camera.transform.position;
        transform.position = _camera.WorldToScreenPoint(movePos);
        gameObject.SetActive(true);
    }
}
