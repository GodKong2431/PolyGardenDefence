using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameGoldUI : MonoBehaviour, IGameGoldObserver
{
    private TextMeshProUGUI _GoldText;

    public void OnGameGoldChanged(int Gold)
    {
        _GoldText.text = 100.ToString();
    }


    private void Awake()
    {
        GameManager.Instance.GameGoldObserver.AddObserver(this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameGoldObserver.RemoveObserver(this);
    }
}