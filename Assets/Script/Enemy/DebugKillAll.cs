using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugClickKill : MonoBehaviour
{
    Camera cam;
    void Awake() => cam = Camera.main;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000f))
            {
                if (hit.collider.TryGetComponent<EnemyBase>(out var enemy))
                {
                    enemy.ApplyDamage(999999f);
                    Debug.Log("[DebugClickKill] Killed " + enemy.name);
                }
            }
        }
    }
}