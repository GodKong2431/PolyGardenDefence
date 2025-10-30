using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameGoldUI : MonoBehaviour, IGameGoldObserver
{
    [Header("GoldUI")]
    [SerializeField]private TextMeshProUGUI _goldText;
    private GameManager gameManager;
    public void OnGameGoldChanged(int Gold)
    {
        _goldText.text = Gold.ToString() + "G";
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
        GameManager.Instance.GameGoldObserver.RemoveObserver(this);
    }


}