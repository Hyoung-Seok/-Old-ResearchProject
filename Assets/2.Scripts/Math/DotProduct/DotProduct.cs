using System;
using TMPro;
using UnityEngine;

public class DotProduct : MonoBehaviour
{
    [SerializeField] private RotateAround target;
    [SerializeField] private TextMeshProUGUI value;
    
    private Vector3 _forward;
    private Vector3 _targetDir;

    private void Start()
    {
        _forward = transform.forward;
    }

    private void Update()
    {
        _targetDir = (target.transform.position - transform.position).normalized;

        var dotProduct = Vector3.Dot(_forward, _targetDir);
        value.text = (Math.Truncate(dotProduct * 100) / 100).ToString();

        if (dotProduct > 0)
        {
            target.ChangeMaterials(0);
            return;
        }
      
        target.ChangeMaterials(1);
    }
}
