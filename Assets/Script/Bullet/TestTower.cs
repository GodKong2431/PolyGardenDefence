using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTower : MonoBehaviour
{
    [SerializeField] BulletManager _bulletManager;
    [SerializeField] BulletType _type;
    WaitForSeconds waitForSeconds;
    [SerializeField] float delay;
    // Start is called before the first frame update

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delay);
    }

    void Start()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            _bulletManager.MakeBullet(_type).gameObject.transform.position = transform.position;
            yield return waitForSeconds;
        }
    }
}
