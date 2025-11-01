using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EffectManager;

public class SoundManager : SingleTon<SoundManager>
{
    //구조체배열
    [SerializeField] private BgmInfo[] _bgm;  
    [SerializeField] private ClipInfo[] _clip;

    [SerializeField] private AudioSource _clipPlayer;

    //구조체 배열 저장해놓을 딕셔너리
    private Dictionary<string, AudioSource> _bgmDict = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioClip> _clipDict = new Dictionary<string, AudioClip>();

    private float _currentVolume = 0.5f;

    public float CurrentVolume
    {
        get { return _currentVolume; }
        set { _currentVolume = value; }
    }


    //이름, 사운드/클립
    [System.Serializable]
    public class BgmInfo
    {
        public string name;
        public AudioSource source;
    }
    [System.Serializable]
    public class ClipInfo
    {
        public string name;
        public AudioClip clip;
    }


    protected override void Awake()
    {
        base.Awake();
        MakeClipDictionary();
    }

    #region
    //구조체배열에 있는 구조체가 딕셔너리에 없으면 딕셔너리에 구조체정보 저장
    private void MakeBgmDictionary()
    {
        if (_bgm == null)
        {
            return;
        }
        foreach (var bgm in _bgm)
        {
            if (!_bgmDict.ContainsKey(bgm.name))
            {
                _bgmDict.Add(bgm.name, bgm.source);
            }
        }
    }
    public void Bgm(string name, bool loop = true)
    {
        StopBgm();
        if (!_bgmDict.TryGetValue(name, out var bgmSource))
        {
            Debug.LogError($"{name} BGM을 찾을 수 없습니다.");
            return;
        }
        //루프 작동
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBgm()//Bgm 멈춤
    {
        foreach (var bgm in _bgmDict.Values)
        {
            bgm.Stop();
        }
    }
    #endregion

    private void MakeClipDictionary()
    {
        if (_clip == null)
        {
            return;
        }
        foreach (var info in _clip)
        {
            if (!_clipDict.ContainsKey(info.name))
            {
                _clipDict.Add(info.name, info.clip);
            }
        }
    }


    public void Clip(string name)
    {
        if (!_clipDict.TryGetValue(name, out var clip))
        {
            Debug.LogError($"{name} Clip을 찾을 수 없습니다.");
            return;
        }
        //클립 작동
        _clipPlayer.PlayOneShot(clip);
    }


    public void BgmVolum(float volume)//볼륨조절
    {
        float clamped = Mathf.Clamp01(volume);

        foreach (var bgm in _bgmDict.Values)
        {
            bgm.volume = clamped;
        }

        _clipPlayer.volume = clamped;
    }
}
