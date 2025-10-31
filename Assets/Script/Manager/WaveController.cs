using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// WaveController
/// - WaveSequenceSO(Stages) 데이터를 순서대로 실행
/// - Stage마다 EnemySpawner.Spawn() 호출 → 진행도(스폰창+딜레이) 0~100% 전송
/// - 딜레이 구간에선 NextWave 버튼(혹은 키)로 즉시 스킵 + 보너스 지급
/// - GameManager에 (MaxWave, Wave증가, Progress) 신호 전달
/// </summary>

public class WaveController : MonoBehaviour
{

    #region === Serialized Refs & Params ===
    [Header("References")]
    [SerializeField] private EnemySpawner _enemySpawner;   // 직렬화로 연결
    [SerializeField] private WaveSequenceSO _sequence;     // 에디터에서 웨이브 데이터 지정

    [Header("Manual Trigger")]
    [SerializeField] private KeyCode _manualKey = KeyCode.Space; // 수동 진행 입력(스페이스 바)
    [SerializeField] private GameObject _nextWaveButton; // 다음 웨이브 시작 버튼

    [Header("Rush Bonus")]
    [SerializeField] private float _rushBonusPerSecond = 5f;      // 초당 보너스 (남은 대기시간 × 이 값)
    #endregion

    #region === Runtime State ===
    private int _stageIndex;
    private Coroutine _sequenceCo;
   
    // 진행도 누적(스폰창 + 딜레이 합친 시간
    private float _accumulated;

    // UI용 상태
    private bool _isInDelay = false;
    private bool _rushRequested = false;
    private float _delayRemain = 0f;
    
    
    // 외부(UI)에서 읽기용
    public bool IsInDelay => _isInDelay;     // UI에서 버튼 활성화 조건으로 사용 가능
    public float DelayRemain => _delayRemain; // 남은 딜레이 표시용
    #endregion


    #region === 0. Entry: Start → RunSequence ===
    private void Start()
    {
        _nextWaveButton.SetActive(false);
        if (!ValidateSetup())
        {
            return;
        }
        // 전체 웨이브 수를 GameManager에 1회 전달
        GameManager.Instance.SetMaxWave(_sequence.Stages.Count);

        _stageIndex = 0;
        _sequenceCo = StartCoroutine(RunSequence());
    }

    /// <summary>
    /// 설정 유효성 체크 & 로그
    /// </summary>
    private bool ValidateSetup()
    {
        if (_enemySpawner == null)
        {
            Debug.LogError("[WaveController] _enemySpawner is null");
            return false;
        }
        if (_sequence == null || _sequence.Stages == null || _sequence.Stages.Count == 0)
        {
            Debug.LogWarning("[WaveController] Wave sequence is empty");
            return false;
        }
        return true;
    }
    #endregion

    #region === 1. 메인 루프: RunSequence ===
    /// <summary>
    /// 전체 웨이브 시퀀스를 순차 실행
    /// 실행 흐름:
    ///  - (대기) → (Stage 시작 알림 & 진행도 0) → SpawnStage → 스폰창 Progress → 딜레이 Progress/스킵 → 100% 보정 → 다음 Stage
    /// </summary>
    private IEnumerator RunSequence()
    {
        if (_sequence.StartDelay > 0f)
            yield return new WaitForSeconds(_sequence.StartDelay);

        while (_stageIndex < _sequence.Stages.Count)
        {
            var stage = _sequence.Stages[_stageIndex];
            if (stage == null || stage.Entries == null || stage.Entries.Count == 0)
            {
                Debug.LogWarning($"[WaveController] Stage[{_stageIndex}] is null or empty. Skip.");
                _stageIndex++;
                continue;
            }

            // 1) 웨이브 시작 알림 + 진행도 0% 초기화
            GameManager.Instance.Wave();                      // 현재 웨이브 +1
            GameManager.Instance.SetWaveProgressPercent(0f);  // 진행도 0%
            _accumulated = 0f;

            // 2) 이번 웨이브(스폰창+딜레이) 시간 구성
            float spawnWindow = CalcSpawnWindowSeconds(stage);
            float delayAfter = Mathf.Max(0f, stage.DelayAfter);
            float totalStage = spawnWindow + delayAfter;
            if (totalStage <= 0f) totalStage = 1f; // 최소 보정


            // 3) 스폰(엔트리 순차)
            yield return SpawnStage(stage);

            // 4) 스폰창 진행도 (0 → 스폰끝)
            yield return UpdateProgressOver(spawnWindow, totalStage);

            // 5) 딜레이 진행도 + 수동 스킵(버튼/키) + 보너스
            yield return InterStageWaitWithManual_Progress(delayAfter, totalStage);

            // 6) 100% 보정
            GameManager.Instance.SetWaveProgressPercent(100f);

            _stageIndex++;
        }
        Debug.Log("[WaveController] All stages completed.");

        // 7) 마지막 웨이브 이후: 필드에 남은 적이 0마리 될 때까지 대기 → Victory
        while (GameManager.Instance != null && GameManager.Instance.AliveEnemies > 0)
        {
            yield return null; // 한 프레임씩 대기
        }
        GameManager.Instance?.Ending();

        _sequenceCo = null;
    }
    #endregion

    #region === 2. Stage 스폰 ===
    /// <summary>
    /// 스테이지 엔트리들을 순차로 스폰.
    /// 실제 인터벌은 EnemySpawner.Spawn 내부에서 처리됨.
    /// </summary>
    private IEnumerator SpawnStage(WaveStage stage)
    {
        foreach (var entry in stage.Entries)
        {
            if (entry == null || entry.Stats == null)
            {
                continue;
            }

            //엔트리별 PreDelay wjrdyd : 동시 진행 가능
            StartCoroutine(SpawnEntryWithPreDelay(entry));
        }

        // 스폰 코루틴들이 내부에서 시간차로 진행되므로 한 프레임 양보
        yield return null;
    }

    private IEnumerator SpawnEntryWithPreDelay(WaveEntry entry)
    {
        if (entry.PreDelay > 0f)
        {
            yield return new WaitForSeconds(entry.PreDelay);
        }

        _enemySpawner.Spawn(
            entry.Stats,
            count: entry.Count,
            pathId: entry.PathId,
            interval: entry.Interval
        );
    }
    #endregion

    #region === 3. 진행도(스폰창 & 딜레이) ===
    /// <summary>
    /// 스폰창 시간 동안 진행도 전송(0~100). 누적(_accumulated)로 totalDuration 대비 비율 계산.
    /// </summary>
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
    ///  - DelayAfter 동안 진행도 마저 채우기
    ///  - 대기 중 클릭/버튼(혹은 키)로 즉시 스킵 → 남은 시간 × 초당보너스 지급
    /// </summary>
    private IEnumerator InterStageWaitWithManual_Progress(float delayAfter, float totalDuration)
    {
        if (delayAfter <= 0f) yield break;

        _isInDelay = true;
        _rushRequested = false;
        _nextWaveButton.SetActive(true);

        float endTime = Time.time + delayAfter;

        while (Time.time < endTime)
        {
            // UI용 남은 시간
            _delayRemain = Mathf.Max(0f, endTime - Time.time);

            // (선택) 키 입력으로도 스킵
            if (Input.GetKeyDown(_manualKey))
                OnClickNextWave();

            // 스킵 요청 시: 보너스 지급 + 진행도 100% 보정 후 종료
            if (_rushRequested)
            {
                int bonus = Mathf.Max(0, Mathf.RoundToInt(_delayRemain * _rushBonusPerSecond));
                if (bonus > 0)
                {
                    GameManager.Instance.AddGold(bonus);
                }

                _accumulated = totalDuration;
                GameManager.Instance.SetWaveProgressPercent(100f);

                _isInDelay = false;
                _rushRequested = false;
                _nextWaveButton.SetActive(false);
                yield break;
            }

            // 진행도(스폰창+딜레이 합산 기준) 계속 채우기
            _accumulated += Time.deltaTime;
            float p = (totalDuration <= 0f) ? 100f : Mathf.Clamp01(_accumulated / totalDuration) * 100f;
            GameManager.Instance.SetWaveProgressPercent(p);

            yield return null;
        }

        _isInDelay = false;
        _rushRequested = false;
        _nextWaveButton.SetActive(false);
    }
    #endregion

    #region === 4. 유틸 & UI Hook ===
    /// <summary>
    /// 현재 Stage의 스폰창(첫 마리 t=0, 나머지 간격 합산) 길이(sec) 계산
    /// </summary>
    private float CalcSpawnWindowSeconds(WaveStage stage)
    {
        float maxEnd = 0f;
        foreach (var entry in stage.Entries)
        {
            if (entry == null)
            {
                continue;
            }

            int count = Mathf.Max(0, entry.Count);
            float interval = Mathf.Max(0f, entry.Interval);
            float preDelay = Mathf.Max(0f, entry.PreDelay);

            // 마지막 스폰 시각(네가 제시한 규칙): PreDelay + Count * Interval
            float end = preDelay + (count * interval);

            if (end > maxEnd)
            {
                maxEnd = end;
            }
        }
        return maxEnd;
    }

    /// <summary>
    /// UI 버튼에서 연결: 딜레이 중 즉시 다음 웨이브로.
    /// 실제 보너스 지급/진행도 보정은 딜레이 루프에서 처리.
    /// </summary>
    public void OnClickNextWave()
    {
        if (_isInDelay) _rushRequested = true;
    }
    #endregion
}