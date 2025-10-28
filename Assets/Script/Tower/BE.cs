using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class BE : MonoBehaviour
{
    //총알 속도
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float deactiveTime = 3f;

    private float bulletDamage;
    private Rigidbody rb;
    private float deactCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void OnEnable()
    {
        ActiveBullet();
    }
    private void Update()
    {
        CountTime();
    }
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
    //총알 날리기 메서드
    private void ActiveBullet()
    {
        deactCount = deactiveTime;

        rb.velocity = Vector3.zero;

        rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);

    }
    //일정시간후에 비활성화 시키는 메서드
    private void CountTime()
    {
        deactCount -= Time.deltaTime;

        if (deactCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    //총알이 벽이나 적에 충돌시 비활성화
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
}
