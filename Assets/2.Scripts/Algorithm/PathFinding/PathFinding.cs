using System.Collections.Generic;

namespace PathFinding
{
    #region BFS
    public class BFS
    {
        // 차례대로 위,아래,왼쪽,오른쪽,좌상,좌하,우상,우하
        private readonly int[] _dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
        private readonly int[] _dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

        public List<(int, int)> BFS_PathFinding(int[,] tile, (int, int) start, (int, int) end)
        {
            var visited = new bool[tile.GetLength(0), tile.GetLength(1)];
            var queue = new Queue<(int, int)>();
            var parent = new (int, int)[tile.GetLength(0), tile.GetLength(1)];

            queue.Enqueue(start);
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if (node == end)
                {
                    return FindPath.FindShortPath(parent, start, end);
                }

                for (var i = 0; i < _dx.Length; ++i)
                {
                    var yPos = node.Item1 + _dy[i];
                    var xPos = node.Item2 + _dx[i];

                    if (xPos < 0 || xPos >= tile.GetLength(1) || yPos < 0 ||
                        yPos >= tile.GetLength(0))
                    {
                        continue;
                    }

                    if (tile[yPos, xPos] == 0 || visited[yPos, xPos] == true)
                    {
                        continue;
                    }

                    if (i >= 4)
                    {
                        if (tile[yPos + _dy[i], xPos] == 0 || tile[yPos, xPos + _dx[i]] == 0)
                        {
                            continue;
                        }
                    }

                    queue.Enqueue((yPos, xPos));
                    visited[yPos, xPos] = true;
                    parent[yPos, xPos] = node;
                }
            }

            return null;
        }
    }
    #endregion

    public class Dijkstra
    {
        private readonly int[] _dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
        private readonly int[] _dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

        public List<(int, int)> Dijkstra_PathFinding(int[,] tile, (int, int) start, (int, int) end)
        {
            var height = tile.GetLength(0);
            var width = tile.GetLength(1);

            var visited = new bool[height, width];
            var distance = new int[height, width];
            var parent = new (int, int)[height, width];

            for (var i = 0; i < height; ++i)
            {
                for (var j = 0; j < width; ++j)
                {
                    distance[i, j] = int.MaxValue;
                }
            }

            var pq = new PriorityQueue<(int, int), int>();
            pq.Enqueue(start, 0);
            visited[start.Item1, start.Item2] = true;
            distance[start.Item1, start.Item2] = 0;

            while (pq.Count > 0)
            {
                pq.TryDequeue(out var node, out var priority);
                visited[node.Item1, node.Item2] = true;

                if (node == end)
                {
                    return FindPath.FindShortPath(parent, start, end);
                }

                for (var i = 0; i < _dx.Length; ++i)
                {
                    var xPos = node.Item2 + _dx[i];
                    var yPos = node.Item1 + _dy[i];
                    
                    if(xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) continue;
                    if(visited[yPos, xPos] == true || tile[yPos, xPos] == 0) continue;
                    if (i >= 4)
                    {
                        if (tile[yPos + _dy[i], xPos] == 0 || tile[yPos, xPos + _dx[i]] == 0)
                        {
                            continue;
                        }
                    }
                    
                    if (priority + 1 > distance[yPos, xPos])
                    {
                        continue;
                    }

                    pq.Enqueue((yPos, xPos), priority + 1);
                    distance[yPos, xPos] = priority + 1;
                    parent[yPos, xPos] = node;
                }
            }

            return null;
        }
    }

    public static class FindPath
    {
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
    }
}
