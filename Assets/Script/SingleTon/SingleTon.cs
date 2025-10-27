using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 전용 제네릭 싱글톤 클래스
/// 게임 전체에서 단 하나의 인스턴스만 존재하도록 보장함
/// 사용 예시: public class SoundManager : SingleTon<SoundManager> { ... }
/// </summary>
public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    // 현재 싱글톤 인스턴스를 저장하는 정적 변수
    private static T _instance;

    /// <summary>
    /// 외부에서 접근 가능한 인스턴스 프로퍼티
    /// Instance가 아직 생성되지 않았다면 자동으로 생성함
    /// </summary>
    public static T Instance
    {
        get
        {
            // 인스턴스가 존재하지 않을 경우 새로 찾거나 생성
            if (_instance == null)
            {
                // 현재 씬에서 T 타입 객체를 탐색
                _instance = FindAnyObjectByType<T>();

                // 만약 씬에 존재하지 않는다면 새 GameObject를 생성해서 추가
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject();
                    _instance = singletonObj.AddComponent<T>();
                    singletonObj.name = typeof(T).ToString();
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// Awake 단계에서 중복 인스턴스가 생기지 않도록 처리
    /// 기존 인스턴스가 있다면 자신은 파괴, 없으면 유지
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // 이미 존재하는 인스턴스가 있을 경우 자신은 제거
            Destroy(gameObject);
        }
        else
        {
            // 최초 생성된 인스턴스로 등록하고, 씬 전환 시에도 파괴되지 않게 설정
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}