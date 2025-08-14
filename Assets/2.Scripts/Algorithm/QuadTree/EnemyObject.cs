using System;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IQuadObject
{
    [Header("Mat")] 
    [SerializeField] private Material enableMat;
    [SerializeField] private Material disableMat;

    private Action _updateAction;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
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
        var obj = new GameObject("Empty");
        Destroy(obj);
    }
}
