using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : SingleTon<Pause>
{
    private bool _isPaused = false;
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape)) //Esc 누를시 Paused작동
    //    {
    //        Paused();
    //        //설정 추가
    //    }

    //    //버튼 혹은 단축키
    //}
    public void Paused()
    {
        _isPaused = !_isPaused; //반전
        if (_isPaused) //True면 일시정지 
        { Time.timeScale = 0f; }
        else //false면 진행
        { Time.timeScale = 1f; }
    }
}
