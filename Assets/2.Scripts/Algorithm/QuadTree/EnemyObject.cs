using System;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    private void Update()
    {
        var obj = new GameObject("Empty");
        Destroy(obj);
    }
}
