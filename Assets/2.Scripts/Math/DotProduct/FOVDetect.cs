using System;
using UnityEngine;

public class FOVDetect : MonoBehaviour
{
    [SerializeField] private float angle = 60f;
    [SerializeField] private RotateAround target;

    private Vector3 _playerForward;
    private float _halfAngle;
    private void Start()
    {
        _playerForward = transform.forward;
        
        // 시야각 절반에 해당하는 값 미리 저장
        _halfAngle = angle * 0.5f;
    }

    private void Update()
    {
        var dir = (target.transform.position - transform.position).normalized;
        
        var dot = Vector3.Dot(_playerForward, dir);
        var fov = Mathf.Cos(_halfAngle * Mathf.Deg2Rad);
        
        DrawView();

        // 시야각 안에 목표 없음
        if (dot < fov)
        {
            target.ChangeMaterials(1);
            return;
        }
        // 시야각 안에 목표 있음
        target.ChangeMaterials(0);
    }

    private void DrawView()
    {
        var pos = transform.position;
        var left = Quaternion.AngleAxis(-_halfAngle, Vector3.up) * _playerForward;
        var right = Quaternion.AngleAxis(_halfAngle, Vector3.up) * _playerForward;
        
        Debug.DrawLine(pos, pos + left * 30f, Color.red);
        Debug.DrawLine(pos, pos + right * 30f, Color.red);
    }
}
