using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] private AudioSource[] _bgm;
    [SerializeField] private AudioSource _clip;
    [SerializeField] public AudioClip[] _clipbgm;
    [SerializeField] private AudioSource[] _clipstop;


    protected override void Awake()
    {
        base.Awake();
        StopBgm();
        StopClip();
        DontDestroyOnLoad(gameObject);
    }
    public void BGM(string name)
    {
        StopBgm(); //모든 Bgm멈춤

        switch (name.ToLower()) //입력된 Bgm 재생
        {
            case "title":
                _bgm[0].loop = true;
                _bgm[0].Play();
                break;
            case "main":
                _bgm[1].loop = true;
                _bgm[1].Play();
                break;
            case "gameover":
                _bgm[2].Play();
                break;
            case "endig":
                _bgm[3].Play();
                break;
            default:
                break;
        }
    }

    public void StopBgm() //Bgm 중지 함수
    {
        for (int i = 0; i < _bgm.Length; i++)
        {
            _bgm[i].Stop();
        }

    }
    public void StopClip()
    {
        for (int i = 0; i < _clipstop.Length; i++)
        {
            _clipstop[i].Stop();
        }
    }
    public void Clip(string name)//포탑에서 발사한 오브젝트 사운드 선탟
    {
        AudioClip clip = null;

        switch (name.ToLower())
        {
            case "bullet":
                clip = _clipbgm[0];
                break;
            case "arrow":
                clip = _clipbgm[1];
                break;
            case "cannon":
                clip = _clipbgm[2];
                break;
            case "magic":
                clip = _clipbgm[3];
                break;
            default:
                break;
        }
        if (clip != null)//선택된 소리 재생
        {
            _clip.PlayOneShot(clip);
        }

    }
    public void BgmVolum(float sound)//볼륨조절
    {
        float volume = Mathf.Clamp01(sound); //0~1 안전 범위
        for (int i = 0; i < _bgm.Length; i++)
        {
            _bgm[i].volume = volume;
        }
        _clip.volume = volume;
    }
}
