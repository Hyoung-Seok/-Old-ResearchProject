using System;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private float speed = 30f;

    [Header("Mat")] 
    [SerializeField] private Material[] materials;
    private Renderer renderer;
    
    private int _prevIndex;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        _prevIndex = 0;
    }

    private void Update()
    {
        transform.RotateAround(anchor.position, Vector3.down, speed * Time.deltaTime);
    }

    public void ChangeMaterials(int index)
    {
        if (_prevIndex == index)
        {
            return;
        }

        _prevIndex = index;
        renderer.material = materials[index];
    }
}
