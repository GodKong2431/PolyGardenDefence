using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum BulletType
{
    Arrow, Ball, Stone, Missile
}

public class Bullet : MonoBehaviour
{
    public BulletType _type { get; set; }
    public float _attack { get; set; }
    public float _speed { get; set; }

    private Rigidbody _rigidBody;       

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ShootBullet();
    }

    protected virtual void ShootBullet()
    {
        _rigidBody.velocity = transform.forward * _speed;
    }
    
    private void OffBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            OffBullet();
        }
    }
}
