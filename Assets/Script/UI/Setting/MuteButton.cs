using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Image _icon;        // 선택
    [SerializeField] private Sprite _onSprite;   // 소리 켜짐
    [SerializeField] private Sprite _offSprite;  // 음소거

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnVolumeChanged += Handle;
            Handle(SoundManager.Instance.CurrentVolume, SoundManager.Instance.IsMuted);
        }
    }
    private void OnDisable()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.OnVolumeChanged -= Handle;
    }
    private void Handle(float vol, bool isMuted)
    {
        if (_icon != null) _icon.sprite = isMuted ? _offSprite : _onSprite;
    }

    // Button.OnClick에 연결
    public void OnClick_Toggle()
    {
        var sm = SoundManager.Instance;
        if (sm == null) return;

        if (sm.IsMuted)
            sm.UnmuteTo(Mathf.Max(Mathf.Epsilon, sm.PreviousVolume)); // ← 이전 볼륨으로 복귀
        else
            sm.Mute();
    }
}