using System;
using UnityEngine;

public class SizeController : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Vector3 scale;
    
    public void ChangeScale()
    {
        obj.transform.localScale = scale;
    }
    
    public void ChangeScale(Vector3 s)
    {
        obj.transform.localScale = s;
    }

    private void Update()
    {
        ChangeScale();
    }
}
