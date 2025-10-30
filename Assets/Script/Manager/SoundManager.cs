using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    public AudioSource _title;
    private AudioSource _main;
    private AudioSource _gameover;
    private AudioSource _endig;
    private AudioSource _clip;
    public AudioClip _bullet;
    public AudioClip _arrow;
    public AudioClip _cannon;
    public AudioClip _magic;

    public void BGM(string name)
    {
        StopBgm(); //모든 Bgm멈춤

        switch (name.ToLower()) //입력된 Bgm 재생
        {
            case "title":
                _title.loop = true;
                _title.Play();
                break;
            case "main":
                _main.loop = true;
                _main.Play();
                break;
            case "gameover":
                _gameover.Play();
                break;
            case "endig":
                _endig.Play();
                break;
            default:
                break;
        }
    }

    public void StopBgm() //Bgm 중지 함수
    {
        _title.Stop();
        _main.Stop();
        _gameover.Stop();
        _endig.Stop();
    }

    public void Clip(string name)//포탑에서 발사한 오브젝트 선탟
    {
        AudioClip clip = null;

        switch (name.ToLower())
        {
            case "bullet":
                clip = _bullet;
                break;
            case "arrow":
                clip = _arrow;
                break;
            case "cannon":
                clip = _cannon;
                break;
            case "magic":
                clip = _magic;
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
        _title.volume = volume;
        _main.volume = volume;
        _gameover.volume = volume;
        _endig.volume = volume;
        _clip.volume = volume;
    }
}
