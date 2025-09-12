using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectionPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Transform rayTransform;
    [SerializeField] private LayerMask groundLayer;
    private void Update()
    {
        var ray = new Ray(rayTransform.position, Vector3.down);

        if (Physics.Raycast(ray, out var hit, 100f, groundLayer) == false)
        {
            return;
        }

        // 투영할 벡터(플레이어의 정면)      
        var forward = transform.forward.normalized;
        // 기준 벡터(평면의 normal)
        var normal = hit.normal;
        //
        // // 투영된 벡터
        // var projection = Vector3.Dot(forward, normal) * normal;
        // // 평면의 normal과 수직인 벡터
        // var dir = forward - projection;
        
        // 함수 사용
        var dir = Vector3.ProjectOnPlane(forward, normal);

        var input = Input.GetAxis("Horizontal");
        transform.Translate(dir * speed * input * Time.deltaTime, Space.World);
        
        // debug
        //Debug.DrawRay(rayTransform.position, projection, Color.red);
        Debug.DrawRay(rayTransform.position, dir, Color.green);
    }
}
