using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    private float _horizontal = 0f;
    private float _vertical = 0f;
    
    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        var dir = new Vector3(_horizontal, 0, _vertical).normalized;

        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }
}
