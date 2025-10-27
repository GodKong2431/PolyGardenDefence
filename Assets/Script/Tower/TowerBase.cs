using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private float range;
    [SerializeField] private Transform firePoint;
    //private string name;
    //private float damage; //총알에서 설정하는게 나을듯
    //private float shotSpeed;//마찬가지
    private float shotDelay = 0.1f;
    private float nextShot = 0;
    private List<Transform> target = new List<Transform>();

    private SphereCollider sphereCollider;
    private GameObject[] bulletPool;
    [SerializeField] private int bulletPoolSize = 15;



    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        Init();
        SetRange();
    }

    private void Update()
    {
        SetTarget();
        AttackTarget();
    }
    //공격범위 설정
    private void SetRange()
    {
        if (sphereCollider != null)
        {
            sphereCollider.radius = range;
        }
    }
    private void SetTarget()
    {
        if (target.Count > 0)
        {
            Transform firstEnemy = target[0];

            if (firstEnemy != null)
            {
                transform.LookAt(firstEnemy);
            }
        }
        else
        {
            target.RemoveAt(0);
        }
    }

    public virtual void AttackTarget()
    {
        if (Time.time < nextShot)
        {
            return;
        }
        nextShot = Time.time + shotDelay;

        foreach (var bullet in bulletPool)
        {
            if (bullet.activeSelf == false)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.SetActive(true);
                return;
            }

        }
    }

    //오브젝트 풀링으로 총알 미리생성, 후에 매니저로 이동
    private void Init()
    {
        bulletPool = new GameObject[bulletPoolSize];

        for (int i = 0; i < bulletPool.Length; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab);
            bulletPool[i].SetActive(false);
        }
    }

    //콜라이더로 들어오는 적 순서대로 리스트에 저장
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Transform enemyTransform = other.transform;

            if (!target.Contains(enemyTransform))
            {
                target.Add(enemyTransform);
            }
        }
    }
    //콜라이더 나가는 순서대로 제거
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Transform enemyTransform = other.transform;

            if (target.Contains(enemyTransform))
            {
                target.Remove(enemyTransform);
            }
        }
    }


}

