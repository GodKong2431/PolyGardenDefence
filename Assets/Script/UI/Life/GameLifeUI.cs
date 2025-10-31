using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLifeUI : MonoBehaviour, IGameLifeObserver
{
    [Header("LifeUI")]
    [SerializeField] private TextMeshProUGUI _lifeText;
    [SerializeField] private Image _lifebar;

    private GameManager gameManager;
    public void OnGameLifeChanged(int currentLife, int maxLife)
    {
        _lifeText.text = currentLife.ToString() + "/" + maxLife.ToString();
        _lifebar.fillAmount = (float)currentLife / maxLife;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.Instance.StartNewRun();
    }

    private void Awake()
    {
        GameManager.Instance.GameLifeObserver.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (gameManager == null)
        {
            return;
        }
        GameManager.Instance.GameLifeObserver.RemoveObserver(this);
    }

    
}
