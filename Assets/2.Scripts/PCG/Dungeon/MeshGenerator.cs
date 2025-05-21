using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private Material[] floorMat;
    [SerializeField] private GameObject horizontalWall;
    [SerializeField] private GameObject verticalWall;

    private static List<Vector3Int> _wallHorizontal = new List<Vector3Int>();
    private static List<Vector3Int> _wallVertical = new List<Vector3Int>();

    public void CreateMesh(NodePosition position)
    {
        if (position == null) return;
        
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
            0,1,2,
            2,1,3
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
        
        for (var col = position.BL.x; col < position.BR.x; ++col)
        {
            var pos = new Vector3Int(col, 0, position.BL.y);
            AddWallPosition(pos, ELine.Horizontal);
        }

        for (var col = position.TL.x; col < position.TR.x; ++col)
        {
            var pos = new Vector3Int(col, 0, position.TL.y);
            AddWallPosition(pos, ELine.Horizontal);
        }

        for (var row = position.BL.y; row < position.TL.y; row++)
        {
            var pos = new Vector3Int(position.BL.x, 0, row);
            AddWallPosition(pos, ELine.Vertical);
        }
        
        for (var row = position.BR.y; row < position.TR.y; row++)
        {
            var pos = new Vector3Int(position.BR.x, 0, row);
            AddWallPosition(pos, ELine.Vertical);
        }
    }
    public void CreateWall()
    {
        foreach (var pos in _wallHorizontal)
        {
            Instantiate(horizontalWall, pos, horizontalWall.transform.rotation, transform);
        }

        foreach (var pos in _wallVertical)
        {
            Instantiate(verticalWall, pos, verticalWall.transform.rotation, transform);
        }
        
        _wallHorizontal.Clear();
        _wallVertical.Clear();
    }

    private void AddWallPosition(Vector3Int pos, ELine dir)
    {
        switch (dir)
        {
          case ELine.Horizontal:
              if (_wallHorizontal.Contains(pos)) _wallHorizontal.Remove(pos);
              else _wallHorizontal.Add(pos);
              break;
          
          case ELine.Vertical:
              if (_wallVertical.Contains(pos)) _wallVertical.Remove(pos);
              else _wallVertical.Add(pos);
              break;
          
          case ELine.None:
          default:
              return;
        }
    }

    private Vector3 ConvertNodePositionToVector3(Vector2Int pos)
    {
        return new Vector3(pos.x, 0, pos.y);
    }
}
