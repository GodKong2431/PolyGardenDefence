using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int life = 100;
    public int gold = 0;
    public int wave = 0;
    private List<IGameGoldObserver> _gameGoldObservers = new List<IGameGoldObserver>();
    private List<IGameLifeObserver> _gameLifeObservers = new List<IGameLifeObserver>();
    private List<IGameWaveObserver> _gameWaveObservers = new List<IGameWaveObserver>();

    public void AddGoldObserver(IGameGoldObserver Observer) //ÀÌ°Å ÀÓ½Ã·Î ¸¸µë ÇÕÄ¥¼öµµ ÀÖÀ½
    {
        _gameGoldObservers.Add(Observer);
    }

    public void RemoveGoldObserver(IGameGoldObserver Observer)
    {
        _gameGoldObservers.Remove(Observer);
    }
    public void AddLifeObserver(IGameLifeObserver Observer)
    {
        _gameLifeObservers.Add(Observer);
    }

    public void RemoveLifeObserver(IGameLifeObserver Observer)
    {
        _gameLifeObservers.Remove(Observer);
    }
    public void AddWaveObserver(IGameWaveObserver Observer)
    {
        _gameWaveObservers.Add(Observer);
    }

    public void RemoveWaveObserver(IGameWaveObserver Observer)
    {
        _gameWaveObservers.Remove(Observer);
    }


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
    void Start()
    {
        
    }
    void Update()
    {
       
    }
    
}
