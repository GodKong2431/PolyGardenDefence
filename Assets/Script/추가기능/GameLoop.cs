using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private bool loop = false;
    // Start is called before the first frame update
    private void Start()
    {

        //스폰 포인트에서 스폰
    }
    public void StartLoop()
    {
        if(!loop)
        {
            StartCoroutine(Loop());
        }
    }
    private IEnumerator Loop()
    {
        loop = true;
        while(GameManager.Instance.life > 0)
        {

        }
    }
    private void StartWave()
    {

    }
}
