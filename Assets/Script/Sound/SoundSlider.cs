using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class SoundSlider : MonoBehaviour
{
    private Slider _slider;
    private bool _mutating;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnVolumeChanged += Handle;
            _mutating = true;
            _slider.value = SoundManager.Instance.CurrentVolume;
            _mutating = false;
        }
        _slider.onValueChanged.AddListener(OnChanged);
    }

    private void OnDisable()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.OnVolumeChanged -= Handle;
        _slider.onValueChanged.RemoveListener(OnChanged);
    }

    private void Handle(float vol, bool isMuted)
    {
        _mutating = true;
        _slider.value = vol;     // 버튼으로 Mute/Unmute 시 자동 반영
        _mutating = false;
    }

    private void OnChanged(float v)
    {
        if (_mutating || SoundManager.Instance == null) return;

        if (v <= 0f) SoundManager.Instance.Mute();       //  슬라이더 0 → 뮤트
        else SoundManager.Instance.UnmuteTo(v);  // 뮤트 중 움직이면 언뮤트(+해당 값)
    }

}
