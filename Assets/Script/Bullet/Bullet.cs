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

    Rigidbody _rb;


    // Start is called before the first frame update

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rb.velocity = Vector3.zero;
    }

    void Start()
    {
        _rb.velocity = transform.forward * _speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OffBullet()
    {

    }

}
