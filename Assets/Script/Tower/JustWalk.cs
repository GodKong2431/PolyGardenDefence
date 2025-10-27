using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustWalk : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ammo")
        {

            Debug.Log($"¸Â¾ÒÀ½"); 
        }
    }
}
