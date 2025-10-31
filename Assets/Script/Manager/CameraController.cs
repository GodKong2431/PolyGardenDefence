using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _isMovement = true;
    private float scroll;

    [Header("MoveSpeed")]
    [SerializeField] private float _speed = 30f;

    [Header("BorderThickness")]
    [SerializeField] private float _borderThickness = 10f;

    [Header("Scroll")]
    [SerializeField] private float _scrollSpeed = 5f;
    [SerializeField] private float _scrollMinY = 5f;
    [SerializeField] private float _scrollMaxY = 30f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 80f;
    void Update()
    {
        // 이동 제약
        if (Input.GetKeyDown(KeyCode.P))
        {
            _isMovement = !_isMovement;
        }
        if (!_isMovement)
        {
            return;
        }

        // 방향값 초기화
        Vector3 _moveDir = Vector3.zero;

        // 카메라가 바라보는 방향(전후좌우 기준)
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // 수평 방향으로만 이동 (Y축 방향 제거)
        forward.y = 0;
        right.y = 0;


        // 4방향 탐지 (WASD + 마우스 위치)
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - _borderThickness)
        {
            _moveDir += forward;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= _borderThickness)
        {
            _moveDir -= forward;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - _borderThickness)
        {
            _moveDir += right;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= _borderThickness)
        {
            _moveDir -= right;
        }

        // 탐지된 방향으로 이동
        if (_moveDir != Vector3.zero)
        {
            transform.position += _moveDir.normalized * _speed * Time.deltaTime;
        }

        if (Input.GetKey("q"))
        {
            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("e"))
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
        }

        // 마우스 휠 감지
        scroll = Input.GetAxis("Mouse ScrollWheel");

        // 줌 인/아웃 (카메라 시선 방향 기준)
        if ((transform.position.y > _scrollMinY || scroll < 0) &&
            (transform.position.y < _scrollMaxY || scroll > 0))
        {
            transform.position += transform.forward * scroll * _scrollSpeed * 1000f * Time.deltaTime;
        }

        // 최소/최대 높이 제한
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, _scrollMinY, _scrollMaxY),
            transform.position.z
        );
    }
}
