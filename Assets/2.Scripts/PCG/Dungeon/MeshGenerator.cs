using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private Material[] floorMat;

    public void CreateMesh(NodePosition position)
    {
        var vertices = new Vector3[]
        {
            ConvertNodePositionToVector3(position.TL),
            ConvertNodePositionToVector3(position.TR),
            ConvertNodePositionToVector3(position.BL),
            ConvertNodePositionToVector3(position.BR)
        };

        var uvs = new Vector2[vertices.Length];
        for (var i = 0; i < uvs.Length; ++i)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        var triangles = new int[]
        {
            0,1,2,2,1,3
        };

        var mesh = new Mesh
        {
            vertices = vertices,
            uv = uvs,
            triangles = triangles
        };

        var floor = new GameObject(position.BL + "Mesh", 
            typeof(MeshFilter), typeof(MeshRenderer));
        
        floor.transform.SetParent(transform);
        floor.GetComponent<MeshFilter>().mesh = mesh;
        floor.GetComponent<MeshRenderer>().material = floorMat[Random.Range(0, floorMat.Length)];
    }

    private Vector3 ConvertNodePositionToVector3(Vector2Int pos)
    {
        return new Vector3(pos.x, 0, pos.y);
    }
}
