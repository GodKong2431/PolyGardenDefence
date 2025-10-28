using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class GameManager : SingleTon<GameManager>
{
    //Life 관련 필드
    private int _maxLife = 100;
    private int _currentLife = 0;

    //Gold 관련 필드
    private int _gold = 0;

    //Wave 관련 필드
    private int _currentWave = 0;
    private int _maxWave = 10;
    private float _progress = 0;



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
            waveObserver.OnGameWaveChanged(_currentWave, _progress);
        }
    }
    #endregion


    public void OnEnemyKilled(int bounty) //골드 획득
    {
        AddGold(bounty);
    }

    public void AddGold(int add) //OnEnemyKilled 랑 합칠까 생각중
    {
        _gold += add;
        //ui에서 출력
    }

    public void Life(bool sublife)
    {
        if(sublife == true)
        {
            if(_currentLife > 0)
            {
                _currentLife--;
                //ui에서 출력
            }
            else
            {
                GameOver(true);
            }
            
        }
    }
    public void Wave()
    {
        if (_currentWave <= _maxWave)
        {
            Ending();
        }
        else
        {
            _currentWave++;
        }
    }

    public void GameOver(bool Die)
    {
        //ui 출력
    }
    public void Ending() 
    {
        
    }
}
