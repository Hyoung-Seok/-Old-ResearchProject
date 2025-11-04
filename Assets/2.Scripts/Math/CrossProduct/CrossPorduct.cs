using System;
using UnityEngine;

public class CrossPorduct : MonoBehaviour
{
    [SerializeField] private RotateAround target;

    private void Update()
    {
        var forward = transform.forward;
        var dir = (target.transform.position - transform.position).normalized;

        var cross = Vector3.Cross(forward, dir);
        var dot = Vector3.Dot(cross, Vector3.up);
        
        // 우측에 존재
        if (dot > 0)
        {
            target.ChangeMaterials(0);
            return;
        }
        
        target.ChangeMaterials(1);
    }
}
