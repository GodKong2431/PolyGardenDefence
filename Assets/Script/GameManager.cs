using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int life = 100;
    public int gold = 0;
    public int wave = 0;

    void OnEnemyKilled(int bounty) //°ñµå È¹µæ
    {
        gold += bounty; //ui ³ª¿À¸é Ãß°¡
    }
    void Life(bool sublife)
    {
        if(sublife == true)
        {
            if(life > 0)
            {
                life--;
                //uiÇ¥½Ã
            }
            else
            {
                GameOver(true);
            }
            
        }
    }
    void Wave()
    {
        wave++;
    }

    void GameOver(bool Die)
    {
        //ui Ãâ·Â
    }
    void Ending() 
    {

    }
    void Start()
    {
        
    }
    void Update()
    {
       
    }
    
}
