using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public int[,] ArrayGraph { get; } =
    {
        {0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 0 → 1, 2
        {0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0}, // 1 → 3, 4
        {0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0}, // 2 → 7, 8
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 3 (자식 없음)
        {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0}, // 4 → 5, 6
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 5 (자식 없음)
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 6 (자식 없음)
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0}, // 7 → 9, 10
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}, // 8 → 11
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 9 (자식 없음)
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, // 10 (자식 없음)
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}  // 11 (자식 없음)
    };

    public List<List<int>> ListGraph { get; } = new List<List<int>>()
    {
        new List<int>() { 1, 2 },
        new List<int>() { 3, 4 },
        new List<int>() { 7, 8 },
        new List<int>(),
        new List<int>() { 5, 6 },
        new List<int>(),
        new List<int>(),
        new List<int>() { 9, 10 },
        new List<int>() { 11 },
        new List<int>(),
        new List<int>(),
        new List<int>()
    };

    public readonly int ColLength;
    public readonly int RowLength;

    public Graph()
    {
        ColLength = ArrayGraph.GetLength(0);
        RowLength = ArrayGraph.GetLength(1);
    }
}
