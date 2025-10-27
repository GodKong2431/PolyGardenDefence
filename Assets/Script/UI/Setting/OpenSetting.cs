using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSetting : MonoBehaviour
{
    [SerializeField]private Canvas _settingCanvas;

    private void Awake()
    {
        _settingCanvas = Instantiate(_settingCanvas);
    }

    public void ChangeSettingCanvasActive()
    {


        if(!_settingCanvas.gameObject.activeSelf)
        {
            _settingCanvas.gameObject.SetActive(true);
            return;
        }
        else
        {
            _settingCanvas.gameObject.SetActive(false);
            return;
        }
    }
}
