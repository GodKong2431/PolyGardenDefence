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
    [SerializeField] private UnityEvent<int> _onRushBonus;

    private int _stageIndex;
    private Coroutine _sequenceCo;
    private float _accumulated;


    private bool _isInDelay = false;
    private bool _rushRequested = false;
    private float _delayRemain = 0f;

    public bool IsInDelay => _isInDelay;     // UI에서 버튼 활성화 조건으로 사용 가능
    public float DelayRemain => _delayRemain; // 남은 딜레이 표시용


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
        GameManager.Instance.SetMaxWave(_sequence.Stages.Count);
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

            GameManager.Instance.Wave();                      // 현재 웨이브 +1
            GameManager.Instance.SetWaveProgressPercent(0f);  // 진행도 0% 초기화

            //이번 웨이브의 시간 구성 계산
            float spawnWindow = CalcSpawnWindowSeconds(stage);
            float delayAfter = Mathf.Max(0f, stage.DelayAfter);
            float totalStage = spawnWindow + delayAfter;
            if (totalStage <= 0f) totalStage = 1f; // 최소 보정


            // 1) 스테이지 스폰(순차)
            yield return SpawnStage(stage);

            // 2) "스폰창 구간" 동안 진행도 갱신
            yield return UpdateProgressOver(spawnWindow, totalStage);

            // 3) "딜레이 구간" 동안 진행도 갱신 (+ 수동 러시 지원)
            yield return InterStageWaitWithManual_Progress(delayAfter, totalStage);

            // 4) 안전 보정(정확히 100% 되도록)
            GameManager.Instance.SetWaveProgressPercent(100f);
        }

        Debug.Log("[WaveController] All stages completed.");
    }

    /// <summary>
    /// 스테이지 엔트리들을 순차로 스폰
    /// </summary>
    private IEnumerator SpawnStage(WaveStage stage)
    {
        // 1) 스폰 지시
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

        // 2) 시간 기준 진행도 계산
        float stageSeconds = 0f;
        foreach (var e in stage.Entries)
        {
            if (e == null) continue;
            int count = Mathf.Max(0, e.Count);
            float interval = Mathf.Max(0f, e.Interval);
            // 첫 마리는 t=0에 나오므로 (count-1) * interval 이 실제 소요 시간
            stageSeconds += Mathf.Max(0, count - 1) * interval;
        }
        if (stageSeconds <= 0f) stageSeconds = 1f; // 최소 보정

        // 3) 0→100% 매 프레임 전송
        float elapsed = 0f;
        while (elapsed < stageSeconds)
        {
            float percent = (elapsed / stageSeconds) * 100f;
            GameManager.Instance.SetWaveProgressPercent(percent); // 매 프레임
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 4) 마무리 보정
        GameManager.Instance.SetWaveProgressPercent(100f);
        yield return null;
    }


    private float CalcSpawnWindowSeconds(WaveStage stage)
    {
        float s = 0f;
        foreach (var e in stage.Entries)
        {
            if (e == null) continue;
            int count = Mathf.Max(0, e.Count);
            float interval = Mathf.Max(0f, e.Interval);
            // 첫 마리는 t=0에 나오므로 (count-1) * interval 이 실제 소요 시간
            s += Mathf.Max(0, count - 1) * interval;
        }
        return s;
    }

    private IEnumerator UpdateProgressOver(float segmentDuration, float totalDuration)
    {
        float d = Mathf.Max(0f, segmentDuration);
        float t = 0f;
        while (t < d)
        {
            _accumulated += Time.deltaTime;
            float p = (totalDuration <= 0f) ? 100f : Mathf.Clamp01(_accumulated / totalDuration) * 100f;
            GameManager.Instance.SetWaveProgressPercent(p);
            t += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 스테이지 사이 대기:
    /// - DelayAfter 만큼 기다리면 자동 진행
    /// - 대기 중 마우스/버튼 입력 시 즉시 다음 스테이지로 넘어가고 보너스 지급
    /// </summary>
    private IEnumerator InterStageWaitWithManual_Progress(float delayAfter, float totalDuration)
    {
        if (delayAfter <= 0f)
            yield break;

        _isInDelay = true;
        _rushRequested = false;

        float t = 0f;
        while (t < delayAfter)
        {
            _delayRemain = Mathf.Max(0f, delayAfter - t);

            // (선택) 키 입력도 스킵 허용
            if (Input.GetKeyDown(_manualKey))
                OnClickNextWave(); // 버튼과 동일 경로로 처리

            // 버튼/키로 스킵 요청
            if (_rushRequested)
            {
                int bonus = Mathf.Max(0, Mathf.RoundToInt(_delayRemain * _rushBonusPerSecond));
                if (bonus > 0) GameManager.Instance.AddGold(bonus); // 직접 지급

                // 진행도 100%로 보정
                _accumulated = totalDuration;
                GameManager.Instance.SetWaveProgressPercent(100f);

                _isInDelay = false;
                _rushRequested = false;
                yield break;
            }

            // 진행도(스폰+딜레이 총합 기준) 갱신
            _accumulated += Time.deltaTime;
            float p = (totalDuration <= 0f) ? 100f : Mathf.Clamp01(_accumulated / totalDuration) * 100f;
            GameManager.Instance.SetWaveProgressPercent(p);

            t += Time.deltaTime;
            yield return null;
        }

        _isInDelay = false;
        _rushRequested = false;
    }

    /// <summary>
    /// UI 버튼에서 호출할 수동 진행용 래퍼
    /// </summary>
    public void OnClickNextWave()
    {
        if (!_isInDelay) return;
        _rushRequested = true;  // 딜레이 루프가 이 플래그를 보고 즉시 종료 + 보너스 지급
    }
}