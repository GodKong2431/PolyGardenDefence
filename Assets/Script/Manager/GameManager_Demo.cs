using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 코인/점수/목숨을 중앙에서 관리하는 매니저(최소 버전)
/// </summary>
public class GameManager_Demo : SingleTon<GameManager_Demo>
{
    [Header("Game Rule")]
    [SerializeField] private int _lives = 10;
    [SerializeField] private int _coins = 100;
    [SerializeField] private int _score = 0;

    public int Lives => _lives;
    public int Coins => _coins;
    public int Score => _score;

    protected override void Awake()
    {
        base.Awake();
    }

    public bool UseCoins(int cost)
    {
        if (_coins < cost)
        {
            return false;
        }

        _coins -= cost;
        // TODO: UI 갱신 이벤트 발행
        return true;
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _coins += amount;
        // TODO: UI 갱신 이벤트 발행
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _score += amount;
        // TODO: UI 갱신 이벤트 발행
    }

    /// <summary>
    /// 적 처치 시 호출: 코인/점수 증가
    /// </summary>
    public void OnEnemyKilled(int bounty)
    {
        AddCoins(bounty);
        AddScore(bounty * 10);
    }

    /// <summary>
    /// 적이 골인했을 때 호출: 보스면 즉시 패배, 아니면 라이프 1 감소
    /// </summary>
    public void OnEnemyGoal(bool isBoss)
    {
        if (isBoss)
        {
            GameOver();
            return;
        }

        _lives--;
        if (_lives <= 0)
        {
            GameOver();
        }
        else
        {
            // TODO: UI 갱신 이벤트 발행
        }
    }

    private void GameOver()
    {
        // TODO: 결과 UI 표시, 일시정지 등
        Debug.Log("[GameManager_Demo] Game Over");
        Time.timeScale = 0f;
    }
}