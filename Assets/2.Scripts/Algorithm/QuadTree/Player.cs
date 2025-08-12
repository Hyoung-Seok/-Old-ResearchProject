using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float yClamp = 3f;
    [SerializeField] private LayerMask ground;

    private float _horizontal = 0f;
    private float _vertical = 0f;
    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        var dir = new Vector3(_horizontal, 0, _vertical).normalized;

        var targetPos = transform.position + dir * speed * Time.deltaTime;
        targetPos = SnapYPos(targetPos);

        transform.position = targetPos;
    }

    private Vector3 SnapYPos(Vector3 pos)
    {
        var ray = pos + Vector3.up * 50f;

        if (Physics.Raycast(ray, Vector3.down, out var hit, 500f,ground))
        {
            pos.y = hit.point.y + yClamp;
        }

        return pos;
    }
}
