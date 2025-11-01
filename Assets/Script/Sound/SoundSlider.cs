using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoundSlider : MonoBehaviour
{
    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener
            (value =>{
            if (SoundManager.Instance != null)
                SoundManager.Instance.BgmVolum(value);
            SoundManager.Instance.CurrentVolume = value;
        });
    }

    private void Start()
    {
        _slider.value = SoundManager.Instance.CurrentVolume;
    }


}
