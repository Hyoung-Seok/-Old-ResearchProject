using System;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IQuadObject
{
    [Header("Setting")] 
    [SerializeField] private bool ignoreQuadTree;
    
    [Header("Mat")] 
    [SerializeField] private Material enableMat;
    [SerializeField] private Material disableMat;

    private Action _updateAction;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();

        if (ignoreQuadTree == true)
        {
            EnableObject();
        }
    }

    private void Update()
    {
        _updateAction?.Invoke();
    }

    public void EnableObject()
    {
        _updateAction += PerformanceTestFunction;
        _renderer.material = enableMat;
    }

    public void DisableObject()
    {
        _updateAction -= PerformanceTestFunction;
        _renderer.material = disableMat;
    }

    private void PerformanceTestFunction()
    {
        for (var i = 0; i < 100; ++i)
        {
            var obj = new GameObject("Empty");
            Destroy(obj);
        }
    }
}
