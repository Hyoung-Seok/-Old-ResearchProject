using System;
using UnityEngine;

public class RodriguesRotation : MonoBehaviour
{
    [SerializeField] private Transform axis;
    [SerializeField, Range(0, 360)] private float angle = 0f;
    
    private void Update()
    {
        // RotationAxis();
        
        // // Quaternion.AngleAxis(float angle, Vector3 axis) 사용
        // var axisToObj = transform.position - axis.position;
        // var rotated = Quaternion.AngleAxis(angle, axis.up) * axisToObj;
        // transform.position = axis.position + rotated;

        // transform.RotateAround(Vector3 point, Vector3 axis, float angle)
        transform.RotateAround(axis.position, axis.up, angle);
    }

    private void RotationAxis()
    {
        var axisNormal = axis.up;
        var originToObj = transform.position - axis.position;

        var t = angle * Mathf.Deg2Rad;
        var ct = Mathf.Cos(t);
        var st = Mathf.Sin(t);

        var rotated = originToObj * ct +
                      axisNormal * Vector3.Dot(axisNormal, originToObj) * (1f - ct) +
                      Vector3.Cross(axisNormal, originToObj) * st;
        
        transform.position = axis.position + rotated;
    }
}
