using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMove : MonoBehaviour
{
    [SerializeField] private float _orbitSpeed;
    [SerializeField] private Transform _fixedCrystal;

    // Update is called once per frame
    void Update()
    {
        OrbitalRevolution();
    }
    private void OrbitalRevolution()
    {
        transform.RotateAround(
            _fixedCrystal.position,
            Vector3.up,
            _orbitSpeed * Time.deltaTime

            );
    }
}
