using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWaveUI : MonoBehaviour, IGameWaveObserver
{
    [Header("WaveUI")]
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private Image _wavebar;

    private GameManager gameManager;
    public void OnGameWaveChanged(int currentWave, float progress)
    {
        _waveText.text = currentWave.ToString();
        _wavebar.fillAmount = progress;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Awake()
    {
        GameManager.Instance.GameWaveObserver.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (gameManager == null)
        {
            return;
        }
        GameManager.Instance.GameWaveObserver.RemoveObserver(this);
    }
}
