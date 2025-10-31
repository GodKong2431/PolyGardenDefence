using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingleTon<EffectManager>
{
    [SerializeField] private EffectPrefabInfo[] _effectPrefabInfo;

    private Dictionary<string,GameObject> _effectPrefabs = new Dictionary<string,GameObject>();

    [System.Serializable]
    public struct EffectPrefabInfo
    {
        public string effectName;
        public GameObject prefab;
    }

    protected override void Awake()
    {
        base.Awake();
        MakeEffectDictionary();
    }

    /// <summary>
    /// 구조체배열에 있는 구조체가 딕셔너리에 없으면
    /// 딕셔너리에 구조체정보(프리팹이름,프리팹) 저장
    /// </summary>
    private void MakeEffectDictionary()
    {
        if( _effectPrefabInfo == null)
        {
            return;
        }

        foreach(var info in _effectPrefabInfo)
        {
            if (!_effectPrefabs.ContainsKey(info.effectName))
            {
                _effectPrefabs.Add(info.effectName, info.prefab);
            }
        }
    }
    /// <summary>
    /// 지정된 이름의 이펙트 생성
    /// </summary>
    /// <param name="name">이펙트 프리팹 이름(대소문자 구분필수)</param>
    /// <param name="position">이펙트 생성위치</param>
    /// <param name="rotation">이펙트 회전값</param>
    /// <param name="parent">부모설정(비우면 널)</param>
    /// <returns>생성된 게임 오브젝트(이팩트 프리팹)</returns>
    public GameObject PlayEffect(string name, Vector3 position, Quaternion rotation,Transform parent = null)
    {
        if (!_effectPrefabs.ContainsKey(name))
        {
            Debug.LogError($"{name}은 매니저에 없거나 이름이 틀렸습니다 대/소문자 확인해주세요");
            return null;
        }

        GameObject prefab = _effectPrefabs[name];

        GameObject effectInstance = Instantiate(prefab,position, rotation, parent);

        return effectInstance;
    }

    //이펙트 파괴시킬때 호출
    public void DestroyEffect(GameObject effect)
    {
        if( effect != null)
        {
            Destroy(effect);
        }
    }
}
