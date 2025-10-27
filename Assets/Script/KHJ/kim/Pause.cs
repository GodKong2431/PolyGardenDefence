using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPaused = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Esc 누를시 Paused작동
        {
            Paused();
        }
    }
    public void Paused()
    {
        isPaused = !isPaused; //반전
        if (isPaused) //True면 일시정지 
        { Time.timeScale = 0f; }
        else //false면 진행
        { Time.timeScale = 1f; }
    }
}
