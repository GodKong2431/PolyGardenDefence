using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class WaveController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner _enemySpawner;   // 직렬화로 연결
    [SerializeField] private WaveSequenceSO _sequence;     // 에디터에서 웨이브 데이터 지정

    [Header("Auto Start")]
    [SerializeField] private float _autoStartDelay = 0f;   // 시퀀스 시작 전 대기(초)

    [Header("Manual Trigger")]
    [SerializeField] private KeyCode _manualKey = KeyCode.Mouse0; // 수동 진행 입력(마우스 왼쪽)

    [Header("Rush Bonus")]
    [SerializeField] private float _rushBonusPerSecond = 5f;      // 초당 보너스 (남은 대기시간 × 이 값)
    // 예시: 경제/보상 파트로 보너스 전달 (현재 코드에는 없음, 필요 시 연결)
    [SerializeField] private UnityEvent<int> _onRushBonus;

    private int _stageIndex;
    private Coroutine _sequenceCo;

    private void Start()
    {
        if (_enemySpawner == null)
        {
            Debug.LogError("[WaveController] _enemySpawner is null");
            return;
        }
        if (_sequence == null || _sequence.Stages == null || _sequence.Stages.Count == 0)
        {
            Debug.LogWarning("[WaveController] Wave sequence is empty");
            return;
        }

        _stageIndex = 0;
        _sequenceCo = StartCoroutine(RunSequence());
    }

    /// <summary>
    /// 전체 웨이브 시퀀스를 순차 실행
    /// </summary>
    private IEnumerator RunSequence()
    {
        if (_autoStartDelay > 0f)
        {
            yield return new WaitForSeconds(_autoStartDelay);
        }

        while (_stageIndex < _sequence.Stages.Count)
        {
            var stage = _sequence.Stages[_stageIndex];
            if (stage == null || stage.Entries == null || stage.Entries.Count == 0)
            {
                _stageIndex++;
                continue;
            }

            // 1) 스테이지 스폰(순차)
            yield return SpawnStage(stage);

            // 2) 스테이지 간 대기 (자동 또는 수동 당김)
            yield return InterStageWaitWithManual(stage.DelayAfter);

            _stageIndex++;
        }

        Debug.Log("[WaveController] All stages completed.");
    }

    /// <summary>
    /// 스테이지 엔트리들을 순차로 스폰
    /// </summary>
    private IEnumerator SpawnStage(WaveStage stage)
    {
        foreach (var entry in stage.Entries)
        {
            if (entry == null || entry.Stats == null)
            {
                continue;
            }

            _enemySpawner.Spawn(
                entry.Stats,
                count: entry.Count,
                pathId: entry.PathId,
                interval: entry.Interval
            );

            // 필요 시 엔트리 간 텀을 넣고 싶다면 여기에 추가
            // (현재 코드에는 없음, 필요 시 아래 한 줄 주석 해제)
            // yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    /// <summary>
    /// 스테이지 사이 대기:
    /// - DelayAfter 만큼 기다리면 자동 진행
    /// - 대기 중 마우스/버튼 입력 시 즉시 다음 스테이지로 넘어가고 보너스 지급
    /// </summary>
    private IEnumerator InterStageWaitWithManual(float delayAfter)
    {
        if (delayAfter <= 0f)
        {
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < delayAfter)
        {
            // 수동 진행(마우스/버튼)
            if (Input.GetKeyDown(_manualKey))
            {
                // 남은 시간 비례 보너스 계산
                float remain = Mathf.Max(0f, delayAfter - elapsed);
                int bonus = Mathf.Max(0, Mathf.RoundToInt(remain * _rushBonusPerSecond));

                // 보너스 전달(경제/보상 파트에서 구독)
                _onRushBonus?.Invoke(bonus);

                // 예시: 보너스를 직접 지급하고 싶다면 GameManager에 API를 추가하여 호출
                // (현재 코드에는 없음, 필요 시 GameManager_Demo에 메서드 추가)
                // GameManager_Demo.Instance.AddCoins(bonus);

                // 즉시 다음 스테이지로
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// UI 버튼에서 호출할 수동 진행용 래퍼
    /// </summary>
    public void OnClickNextWave()
    {
        // Update 루프의 키 입력과 동일하게 처리하고 싶다면,
        // 여기서도 보너스 계산/지급 로직을 호출해야 함.
        // 간단히 구현하려면: InterStageWaitWithManual 내에서 플래그를 보고 분기하는 구조로 변경 가능.
        // 현재는 키 입력만 지원하므로, 필요 시 UI 경로도 추가 구현.
        // (간단한 방법: 버튼 클릭 시 전역 플래그를 true로 만들어두고 InterStageWaitWithManual에서 체크)
        Debug.Log("[WaveController] OnClickNextWave pressed (UI).");
    }
}