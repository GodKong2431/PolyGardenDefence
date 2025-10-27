using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameGoldUI : MonoBehaviour, IGameGoldObserver
{
    private TextMeshProUGUI _GoldText;
    private GameManager gameManager;
    public void OnGameGoldChanged(int Gold)
    {
        _GoldText.text = 100.ToString();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Awake()
    {
        GameManager.Instance.GameGoldObserver.AddObserver(this);
    }

    private void OnDestroy()
    {

        if(gameManager == null)
        {
            return;
        }

        Debug.Log("°ñµå Á¦°Å");
        GameManager.Instance.GameGoldObserver.RemoveObserver(this);
    }


}