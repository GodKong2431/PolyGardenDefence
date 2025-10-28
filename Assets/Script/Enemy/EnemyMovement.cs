using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaypointPath를 따라 이동하는 적의 이동 컴포넌트.
/// - EnemyBase가 Init 시 경로와 속도를 주입.
/// - Animator가 있을 경우 Speed 파라미터를 자동 업데이트.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float _speed = 2f;
    private float _currentSpeed;
    [Header("Components")]
    [SerializeField] private Animator _anim;  // 직렬화로 연결 권장

    private WaypointPath _path;
    private int _index;
    private bool _isMoving;

    /// <summary>
    /// 이동 경로 및 속도 초기화
    /// </summary>
    public void Init(WaypointPath path, float speed)
    {
        _path = path;
        _speed = speed;
        _currentSpeed = _speed;
        _index = 0;

        if (_path == null || _path.Points.Count == 0)
        {
            Debug.LogError("[EnemyMovement] Invalid path");
            enabled = false;
            return;
        }

        transform.position = _path.Points[0].position;
        _isMoving = true;
        enabled = true;

        if (_anim != null)
        {
            _anim.SetFloat("Speed", _speed);
        }
    }

    private void Update()
    {

        if (!_isMoving || _path == null)
        {
            return;
        }

        if (_index >= _path.Points.Count)
        {
            return;
        }

        MoveToNext();
    }

    /// <summary>
    /// 다음 웨이포인트로 이동 처리
    /// </summary>
    private void MoveToNext()
    {
        var target = _path.Points[_index].position;
        var before = transform.position;


        Vector3 dir = (target - before);
        dir.y = 0f; // 위아래 방향 회전 방지
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 8f);
        }


        transform.position = Vector3.MoveTowards(before, target, _currentSpeed * Time.deltaTime);

        // 애니메이터 Speed 파라미터 업데이트
        if (_anim != null)
        {
            float moveMag = (transform.position - before).magnitude / Time.deltaTime;
            _anim.SetFloat("Speed", moveMag);
        }

        // 목표점 도달 판정
        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            _index++;
            if (_index >= _path.Points.Count)
            {
                OnArriveGoal();
            }
        }
    }

    /// <summary>
    /// 목표 지점 도달 시 호출 (GoalTrigger로 전달)
    /// </summary>
    private void OnArriveGoal()
    {
        _isMoving = false;

        var enemy = GetComponent<EnemyBase>();
        if (enemy == null)
        {
            Debug.LogWarning("[EnemyMovement] EnemyBase not found on goal arrive");
            return;
        }

        GameManager_Demo.Instance.OnEnemyGoal(enemy.Stats.isBoss);
        enemy.Despawn();
    }

    /// <summary>
    /// 이동 중지 (사망, 풀 반환 등)
    /// </summary>
    public void Stop()
    {
        _isMoving = false;
        enabled = false;

        if (_anim != null)
        {
            _anim.SetFloat("Speed", 0f);
        }
    }

    // 이동 재개(공격 끝나고 다시 길 따라 걷기)
    public void Resume()
    {
        _isMoving = true;
        enabled = true;

        if (_anim != null)
            _anim.SetFloat("Speed", _speed);
    }

   

    public void SetSpeed(float newSpeed)
    {
        _currentSpeed = newSpeed;
    }
}