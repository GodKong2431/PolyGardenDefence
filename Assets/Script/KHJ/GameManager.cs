using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class GameManager : SingleTon<GameManager>
{
    public int life = 100;
    public int gold = 0;
    public int wave = 0;



    #region Observer LisnerList
    private Notify<IGameGoldObserver> _gameGoldObserver = new Notify<IGameGoldObserver>();
    public Notify<IGameGoldObserver> GameGoldObserver
    {
        get { return _gameGoldObserver; }
        set { _gameGoldObserver = value; }
    }


    private Notify<IGameLifeObserver> _gameLifeObservers = new Notify<IGameLifeObserver>();
    public Notify<IGameLifeObserver> GameLifeObservers
    {
        get { return _gameLifeObservers; }
        set { _gameLifeObservers = value; }
    }


    private Notify<IGameWaveObserver> _gameWaveObservers = new Notify<IGameWaveObserver>();
    public Notify<IGameWaveObserver> GameWaveObservers
    {
        get { return _gameWaveObservers; }
        set { _gameWaveObservers = value; }
    }
    #endregion




    public void OnEnemyKilled(int bounty) //°ñµå È¹µæ
    {
        AddGold(bounty);
    }

    public void AddGold(int add) //OnEnemyKilled ¶û ÇÕÄ¥±î »ý°¢Áß
    {
        gold += add;
        //ui¿¡¼­ Ãâ·Â
    }

    public void Life(bool sublife)
    {
        if(sublife == true)
        {
            if(life > 0)
            {
                life--;
                //ui¿¡¼­ Ãâ·Â
            }
            else
            {
                GameOver(true);
            }
            
        }
    }
    public void Wave()
    {
        if (wave == 20)
        {
            Ending();
        }
        else
        {
            wave++;
        }
    }

    public void GameOver(bool Die)
    {
        //ui Ãâ·Â
    }
    public void Ending() 
    {
        
    }
}
