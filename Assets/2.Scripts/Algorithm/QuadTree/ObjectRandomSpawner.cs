using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectRandomSpawner : MonoBehaviour
{
    [Header("Spawn Range")]
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius;
    
    [Header("Spawn Setting")]
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private int spawnCount;
    [SerializeField] private float checkSize;
    [SerializeField] private float maxHeight;
    [SerializeField] private float yClamp;
    
    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string objectTag;
    
    public void SpawnObjects()
    {
        var hit = new Collider[10];
        var currentCount = 0;
        var subCount = 0;
        var maxCount = spawnCount * 2;

        while (currentCount < spawnCount || subCount < maxCount)
        {
            var pos = CalculateSpawnPosition();
            var hitCount = Physics.OverlapSphereNonAlloc(pos, checkSize, hit);

            var canSpawn = true;

            for (var i = 0; i < hitCount; ++i)
            {
                if (hit[i] != null && hit[i].CompareTag(objectTag))
                {
                    canSpawn = false;
                    break;
                }
            }

            if (canSpawn)
            {
                pos.y += yClamp;
                var obj = Instantiate(spawnObject, pos, quaternion.identity).GetComponent<EnemyObject>();
                obj.transform.SetParent(transform);

                currentCount++;
            }

            subCount++;
        }
    }

    public void DeleteObjects()
    {
        if (transform.childCount == 0) return;

        for (var i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public List<EnemyObject> GetSpawnObjectsOrNull()
    {
        var objList = new List<EnemyObject>();

        for (var i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).TryGetComponent(out EnemyObject obj) == true)
            {
                objList.Add(obj);
            }
        }

        return objList.Count == 0 ? null : objList;
    }

    private Vector3 CalculateSpawnPosition()
    {
        var randPos = Random.insideUnitCircle * radius;
        var spawnPoint = center + new Vector3(randPos.x, maxHeight, randPos.y);

        var ray = new Ray(spawnPoint, Vector3.down);
        if (Physics.Raycast(ray, out var hit, maxHeight, groundLayer))
        {
            spawnPoint.y = hit.point.y;
        }

        return spawnPoint;
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(center, Vector3.up, radius);
    }
    #endif
}
