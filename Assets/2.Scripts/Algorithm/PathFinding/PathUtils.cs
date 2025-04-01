using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public static class PathUtils
{
    public static event Action<(int, int), Color> ChangeTileColor;
    public static readonly Vector2Int[] SearchDir = new[]
    {
        new Vector2Int(0, -1), new Vector2Int(0, 1),
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1),
        new Vector2Int(1, -1), new Vector2Int(1, 1)
    };
    
    private static readonly int _delayTime = 20;
    
    public static List<(int, int)> FindShortPath((int, int)[,] parent, (int, int) start,
        (int, int) end)
    {
        var result = new List<(int, int)>();
        var current = end;

        while (current != start)
        {
            result.Add(current);
            current = parent[current.Item1, current.Item2];
        }

        result.Add(start);
        result.Reverse();

        return result;
    }

    public static async UniTask InvokeChangeTileColor((int, int) pos, Color color)
    {
        ChangeTileColor?.Invoke(pos, color);
        await UniTask.Delay(_delayTime);
    }
        
    public static bool IsDiagonalBlocked(Tile[,] tile, (int y, int x) curNode, (int y, int x) moveDir)
    {
        var xPos = curNode.x + moveDir.x;
        var yPos = curNode.y + moveDir.y;

        if (xPos < 0 || xPos >= tile.GetLength(1) || yPos < 0 || yPos >= tile.GetLength(0))
            return true;

        return tile[yPos, curNode.x].Weight == (int)EBiome.WALL &&
               tile[curNode.y, xPos].Weight == (int)EBiome.WALL;
    }
}
