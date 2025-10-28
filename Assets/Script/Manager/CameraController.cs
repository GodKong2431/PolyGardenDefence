using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private bool _isMovemont = true;
    private float scroll;

    [Header("MoveSpeed")]
    [SerializeField] private float _speed = 30f;

    [Header("borderThickness")]
    [SerializeField] private float _borderThickness = 10f;

    [Header("Scroll")]
    [SerializeField] private float _scrollSpeed = 5f;
    [SerializeField] private float _scrollMinY = 5f;
    [SerializeField] private float _scrollMaxY = 30f;

    void Update()
    {
        
        //이동 제약
        if (Input.GetKeyDown("p"))
        {
            _isMovemont = !_isMovemont;
        }
        if(!_isMovemont)
        {
            return;
        }

        //방향값 초기화
        Vector3 _moveDir = Vector3.zero;

        //4방향 탐지
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - _borderThickness)
        {
            _moveDir += Vector3.forward;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= _borderThickness)
        {
            _moveDir += Vector3.back;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - _borderThickness)
        {
            _moveDir += Vector3.right;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= _borderThickness)
        {
            _moveDir += Vector3.left;
        }

        //탐지된 방향으로 이동
        if(_moveDir != Vector3.zero)
        {
            transform.Translate(_moveDir.normalized * _speed * Time.deltaTime, Space.World);
        }


        //마우스 휠 감지
        scroll = Input.GetAxis("Mouse ScrollWheel");

        //최소나 최대가 아니면 이동
        if ((transform.position.y > _scrollMinY || scroll < 0) &&
            (transform.position.y < _scrollMaxY || scroll > 0))
        {
            transform.position += transform.forward * scroll * _scrollSpeed * 1000f * Time.deltaTime;
        }

        //최소 최대 정해줌
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, _scrollMinY, _scrollMaxY),
            transform.position.z
            );
    }


}
