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
                    return FindShortPath(parent, start, end);
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

        private List<(int, int)> FindShortPath((int, int)[,] parent, (int, int) start,
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
    #endregion
}
