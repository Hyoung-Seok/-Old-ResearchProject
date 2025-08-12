using System;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    [Header("Mat")] 
    [SerializeField] private Material enableMat;
    [SerializeField] private Material disableMat;

    private Renderer _renderer;
    
    private void Update()
    {
        var obj = new GameObject("Empty");
        Destroy(obj);
    }

    private void OnEnable()
    {
        if (_renderer == null)
        {
            _renderer = gameObject.GetComponent<Renderer>();
        }
        
        _renderer.material = enableMat;
    }

    private void OnDisable()
    {
        if (_renderer == null)
        {
            return;
        }
        
        _renderer.material = disableMat;
    }
}
