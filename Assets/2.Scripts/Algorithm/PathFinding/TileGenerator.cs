using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class TileGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ground;

    [Header("Setting")] 
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float offset = 0.5f;
    
    private readonly int[] _dx = new[] { 0, 0, -2, 2 };
    private readonly int[] _dy = new[] { -2, 2, 0, 0 };
    private int[,] _tile;
    
    private void Start()
    {
        if (width % 2 == 0) width -= 1;
        if (height % 2 == 0) height -= 1;
        
        SetTileArray();
        GenerateTile();
    }

    private void SetTileArray()
    {
        _tile = new int[height, width];
        var stack = new Stack<(int, int)>();
        
        stack.Push(GetRandomPosition());

        while (stack.Count > 0)
        {
            var pos = stack.Pop();
            _tile[pos.Item1, pos.Item2] = 1;

            var nextPos = GetNextPosition(pos);

            if (nextPos.Item1 == -1)
            {
                continue;
            }
            
            stack.Push(pos);
            stack.Push(nextPos);
        }
    }

    private (int, int) GetNextPosition((int, int) pos)
    {
        var land = new[] { 0, 1, 2, 3 };
        land = land.OrderBy(_ => Random.value).ToArray();

        foreach (var index in land)
        {
            if (pos.Item1 + _dy[index] < 1 || pos.Item1 + _dy[index] >= height - 1 ||
                pos.Item2 + _dx[index] < 1 || pos.Item2 + _dx[index] >= width - 1 ||
                _tile[pos.Item1 + _dy[index], pos.Item2 + _dx[index]] == 1)
            {
                continue;
            }

            _tile[pos.Item1 + _dy[index] / 2, pos.Item2 + _dx[index] / 2] = 1;
            return (pos.Item1 + _dy[index], pos.Item2 + _dx[index]);
        }

        return (-1, -1);
    }
    
    private (int, int) GetRandomPosition()
    {
        var pos = (0, 0);

        pos.Item1 = Random.Range(0, (height - 1) / 2) * 2 + 1;
        pos.Item2 = Random.Range(0, (width - 1) / 2) * 2 + 1;
        
        return pos;
    }

    private void GenerateTile()
    {
        var pos = Vector3.zero;
        
        for (var col = 0; col < height; ++col)
        {
            for (var row = 0; row < width; ++row)
            {
                var obj = (_tile[col, row] == 0)
                    ? Instantiate(wall, transform)
                    : Instantiate(ground, transform);

                obj.transform.position = pos;
                pos.x += offset;
            }

            pos.x = 0;
            pos.z -= offset;
        }
    }
}
