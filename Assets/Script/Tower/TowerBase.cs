using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float range;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected string name = "base tower";
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float shotDelay = 0.5f;

    protected float nextShot = 0;
    protected List<Transform> target = new List<Transform>();

    protected SphereCollider sphereCollider;
    protected GameObject[] bulletPool;
    [SerializeField] protected int bulletPoolSize = 15;



    protected virtual void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        Init();
        SetRange();
    }

    protected void Update()
    {
        SetTarget();
        AttackTarget();
    }
    //공격범위 설정
    protected void SetRange()
    {
        if (sphereCollider != null)
        {
            sphereCollider.radius = range;
        }
    }
    protected void SetTarget()
    {
        for(int i = 0; i < target.Count; i++)
        {
            Transform currentTarget = target[i];

            if(currentTarget == null || currentTarget.gameObject.activeSelf==false)
            {
                target.RemoveAt(i);
                i--;
            }
        }

        if (target.Count > 0)
        {
            Transform firstEnemy = target[0];

            if (firstEnemy != null)
            {
                transform.LookAt(firstEnemy);
            }
        }
       
    }

    protected virtual void AttackTarget()
    {
        if(target.Count == 0)
        {
            return;
        }
        if (Time.time < nextShot)
        {
            return;
        }
        nextShot = Time.time + shotDelay;

        foreach (var bullet in bulletPool)
        {
            if (bullet.activeSelf == false)
            {
                BE setBulletDamage = bullet.GetComponent<BE>();

                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                setBulletDamage.SetDamage(damage);
                bullet.SetActive(true);
                return;
            }

        }
    }

    //오브젝트 풀링으로 총알 미리생성, 후에 매니저로 이동
    protected void Init()
    {
        bulletPool = new GameObject[bulletPoolSize];

        for (int i = 0; i < bulletPool.Length; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab);
            bulletPool[i].SetActive(false);
        }
    }

    //콜라이더로 들어오는 적 순서대로 리스트에 저장
    protected void OnTriggerEnter(Collider other)
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
    protected void OnTriggerExit(Collider other)
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

