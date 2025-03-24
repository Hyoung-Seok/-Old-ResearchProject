using System;
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

    private int[,] _tile;

    private void Start()
    {
        SetTileArray();
        GenerateTile();
    }

    private void SetTileArray()
    {
        _tile = new int[height, width];
        
        for (var col = 0; col < height; ++col)
        {
            for (var row = 0; row < width; ++row)
            {
                if (col == 0 || col >= height - 1)
                {
                    _tile[col, row] = 1;
                    continue;
                }

                if (row == 0 || row >= width - 1)
                {
                    _tile[col, row] = 1;
                    continue;
                }

                if (row % 2 == 0 || col % 2 == 0)
                {
                    _tile[col, row] = 1;
                }
            }
        }
        
        for (var col = 1; col < height; col += 2)
        {
            for (var row = 1; row < width; row += 2)
            {
                Vector2Int pos;
                
                if(col == height - 2 && row == width - 2) continue;
                
                if (row == width - 2)
                {
                    pos = new Vector2Int(col + 1, row);
                }
                else if (col == height - 2)
                {
                    pos = new Vector2Int(col, row + 1);
                }
                else if (Random.Range(0, 2) == 0)
                {
                    pos = new Vector2Int(col, row + 1);
                }
                else
                {
                    pos = new Vector2Int(col + 1, row);
                }

                _tile[pos.x, pos.y] = 0;
            }
        }
    }

    private void GenerateTile()
    {
        var pos = Vector3.zero;
        
        for (var col = 0; col < height; ++col)
        {
            for (var row = 0; row < width; ++row)
            {
                var obj = (_tile[col, row] == 1)
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
