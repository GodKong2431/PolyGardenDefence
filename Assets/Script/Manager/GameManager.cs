using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : SingleTon<GameManager>
{
    //Life 관련 필드
    [SerializeField] private int _maxLife = 100;
    private int _currentLife = 0;

    public int CurrentLife => _currentLife;

    //Gold 관련 필드
    [SerializeField] private int _gold = 0;
    [SerializeField] private int _startGold = 100;

    [Header("GameEndCanvas")]
    [SerializeField] private Canvas _victoryCanvas;
    [SerializeField] private Canvas _gameOverCanvas;



    //Wave 관련 필드
    private int _currentWave = 0;
    private int _maxWave = 2;
    private float _progress = 0;

    [SerializeField] private int _aliveEnemies;
    public int AliveEnemies => _aliveEnemies;

    // 적 스폰 시 +1
    public void RegisterEnemySpawned()
    {
        _aliveEnemies++;
    }

    // 적이 완전히 사라질 때 -1
    public void RegisterEnemyRemoved()
    {
        if (_aliveEnemies > 0)
        {
            _aliveEnemies--;
        }
    }

    #region Observer LisnerList
    //Gold 옵저버 생성
    private Notify<IGameGoldObserver> _gameGoldObserver = new Notify<IGameGoldObserver>();
    //Gold 옵저버 프로퍼티
    public Notify<IGameGoldObserver> GameGoldObserver
    {
        get { return _gameGoldObserver; }
    }
    //Gold Lisner 실행
    private void NotifyGoldUpdate()
    {
        foreach (IGameGoldObserver goldObserver in _gameGoldObserver.NotifyList)
        {
            goldObserver.OnGameGoldChanged(_gold);
        }
    }

    //Life 옵저버 생성
    private Notify<IGameLifeObserver> _gameLifeObservers = new Notify<IGameLifeObserver>();
    //Life 옵저버 프로퍼티
    public Notify<IGameLifeObserver> GameLifeObserver
    {
        get { return _gameLifeObservers; }
    }
    //Life Lisner 실행
    private void NotifyLifeUpdate()
    {
        foreach (IGameLifeObserver lifeObserver in _gameLifeObservers.NotifyList)
        {
            lifeObserver.OnGameLifeChanged(_currentLife, _maxLife);
        }
    }

    //Wave 옵저버 생성
    private Notify<IGameWaveObserver> _gameWaveObservers = new Notify<IGameWaveObserver>();
    //Wave 옵저버 프로퍼티
    public Notify<IGameWaveObserver> GameWaveObserver
    {
        get { return _gameWaveObservers; }
    }
    //Wave Lisner 실행
    private void NotifyWaveUpdate()
    {
        foreach (IGameWaveObserver waveObserver in _gameWaveObservers.NotifyList)
        {
            waveObserver.OnGameWaveChanged(_currentWave, _maxWave, _progress);
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _currentLife = _maxLife;
    }
    public void OnEnemyKilled(int bounty) //적 처치시 골드 획득
    {
        AddGold(bounty);
    }

    public void AddGold(int add)// 골드 증가
    {
        _gold += add;
        NotifyGoldUpdate();
        Debug.Log("골드 증가");
        //ui에서 출력
    }
    public bool SubGold(int sub)
    {
        if (_gold >= sub)
        {
            _gold -= sub;
            NotifyGoldUpdate();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Life(bool sublife, bool isboss = false)
    {
        if (isboss)
        {
            GameOver();
            Debug.Log("보스로 인한 사망");
            return; // 보스면 여기서 종료
        }

        if (sublife)
        {
            if (_currentLife > 0) // 기존: >= 0
            {
                _currentLife--;
                NotifyLifeUpdate();
                Debug.Log("현재 남은 라이프 : " + _currentLife);

                if (_currentLife == 0)
                {
                    GameOver();
                }
            }
        }
    }
    public void Wave()
    {
        _currentWave++;
        // 마지막 웨이브여도 여기서 Ending() 부르지 않음
        NotifyWaveUpdate();
        Debug.Log("현재 웨이브 : " + _currentWave);
    }

    public void GameOver()
    {
        //ui 출력
        Debug.Log("게임 오버");
        Instantiate(_gameOverCanvas);
        Time.timeScale = 0f;
        //Pause.Instance.Paused();
        //SceneManager.LoadScene("");
        //if (Input.anyKeyDown)
        //{
        //    SceneManager.LoadScene("Title");
        //}
    }
    public void Ending()
    {
        Debug.Log("엔딩");
    
        Paused();
        Instantiate(_victoryCanvas);
    }

    // 퍼센트 0~100 보정해서 저장
    public void SetWaveProgressPercent(float percent)
    {
        _progress = Mathf.Clamp(percent, 0f, 100f);
    }

    // 매 프레임 UI 푸시
    private void Update()
    {
        NotifyWaveUpdate();
    }

    public void SetMaxWave(int maxWave)
    {
        _maxWave = Mathf.Max(0, maxWave);
        // NotifyWaveUpdate();
    }

    public void Paused()
    {
        if (Time.timeScale == 0f) //True면 일시정지 
        { Time.timeScale = 1f; }
        else //false면 진행
        { Time.timeScale = 0f; }
    }

    public void StartNewRun()
    {
        // 일시정지 해제(이전 Victory/GameOver에서 멈춰 있었을 수 있음)
        Time.timeScale = 1f;

        // 코어 상태 초기화
        _currentLife = _maxLife;
        _gold = _startGold;
        _currentWave = 0;
        _progress = 0f;
        _aliveEnemies = 0;

        // UI 즉시 갱신
        NotifyLifeUpdate();
        NotifyGoldUpdate();
        NotifyWaveUpdate();

        Debug.Log("[GameManager] StartNewRun: state reset");
    }
}
