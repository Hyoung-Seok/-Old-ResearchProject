using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PathFinding
{
    public enum EFindingType
    {
        BFS,
        Dijkstra,
        AStar
    }
    
    public class BFS
    {
        private readonly int[] _dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
        private readonly int[] _dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

        public async UniTask<List<(int, int)>> BFS_PathFinding(Tile[,] tile, (int, int) start, (int, int) end)
        {
            var visited = new bool[tile.GetLength(0), tile.GetLength(1)];
            var queue = new Queue<(int, int)>();
            var parent = new (int, int)[tile.GetLength(0), tile.GetLength(1)];

            queue.Enqueue(start);
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                //await FindPath.InvokeChangeTileColor(node, Color.gray);

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

                    if (tile[yPos, xPos].Weight == 0 || visited[yPos, xPos] == true)
                    {
                        continue;
                    }

                    if (i >= 4 && FindPath.IsDiagonalBlocked(tile, node, (_dy[i], _dx[i])) == true)
                    {
                        continue;
                    }

                    queue.Enqueue((yPos, xPos));
                    visited[yPos, xPos] = true;
                    parent[yPos, xPos] = node;
                }
            }

            return null;
        }
    }

    public class Dijkstra
    {
        private readonly int[] _dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
        private readonly int[] _dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

        public async UniTask<List<(int, int)>> Dijkstra_PathFinding(Tile[,] tile, (int, int) start, (int, int) end)
        {
            var height = tile.GetLength(0);
            var width = tile.GetLength(1);

            var visited = new bool[height, width];
            var distance = new float[height, width];
            var parent = new (int, int)[height, width];

            for (var i = 0; i < height; ++i)
            {
                for (var j = 0; j < width; ++j)
                {
                    distance[i, j] = int.MaxValue;
                }
            }

            var pq = new PriorityQueue<(int, int), float>();
            pq.Enqueue(start, 0);
            distance[start.Item1, start.Item2] = 0;

            while (pq.Count > 0)
            {
                pq.TryDequeue(out var node, out var priority);

                if(visited[node.Item1, node.Item2] == true) continue;
                
                if (node == end)
                {
                    return FindPath.FindShortPath(parent, start, end);
                }
                
                visited[node.Item1, node.Item2] = true;

                for (var i = 0; i < _dx.Length; ++i)
                {
                    var xPos = node.Item2 + _dx[i];
                    var yPos = node.Item1 + _dy[i];
                    
                    if(xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) continue;
                    if(visited[yPos, xPos] == true || tile[yPos, xPos].Weight == (int)EBiome.WALL) continue;
                    
                    // 대각선 이동 가능 검사
                    if (i >= 4 && FindPath.IsDiagonalBlocked(tile, node, (_dy[i], _dx[i])) == true)
                    {
                        continue;
                    }

                    var weight = (i >= 4) ? tile[yPos, xPos].Weight * 1.4f : tile[yPos, xPos].Weight;
                    var newDistance = weight + priority;
                    
                    if (newDistance > distance[yPos, xPos])
                    {
                        continue;
                    }

                    pq.Enqueue((yPos, xPos), newDistance);
                    distance[yPos, xPos] = newDistance;
                    parent[yPos, xPos] = node;
                }
            }

            return null;
        }
    }

    public class AStar
    {
        private readonly Vector2Int[] _searchDir = new[]
        {
            new Vector2Int(0, -1), new Vector2Int(0, 1),
            new Vector2Int(-1, 0), new Vector2Int(1, 0),
            new Vector2Int(-1, -1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(1, 1)
        };

        public async UniTask<List<(int, int)>> AStar_PathFinding(Tile[,] tile, (int, int) start, (int, int) end)
        {
            var height = tile.GetLength(0);
            var width = tile.GetLength(1);
            
            var openSet = new PriorityQueue<NodeData, float>();
            var openSetDic = new Dictionary<(int, int), NodeData>();
            var closeSet = new bool[height, width];
            var parent = new (int, int)[height, width];

            // 첫 노드는 바로 close 처리 되기 때문에 OpenSetDic에 들어갈 필요 없음
            openSet.Enqueue(new NodeData(start, 0, 0), 0);

            while (openSet.Count > 0)
            {
                if (openSet.TryDequeue(out var curNode, out var f) == false) return null;
                if (curNode.Pos == end)
                {
                    return FindPath.FindShortPath(parent, start, end);
                }

                closeSet[curNode.Pos.Item1, curNode.Pos.Item2] = true;

                var current = -1;
                
                foreach (var dir in _searchDir)
                {
                    current++;
                    var xPos = curNode.Pos.Item2 + dir.x;
                    var yPos = curNode.Pos.Item1 + dir.y;
                    
                    if(xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) continue;
                    if(closeSet[yPos, xPos] == true || tile[yPos, xPos].Weight == (int)EBiome.WALL) continue;
                    
                    //  대각선 이동이 가능한지 확인
                    if (current >= 4 && FindPath.IsDiagonalBlocked(tile, curNode.Pos, (dir.y, dir.x)) == true)
                    {
                        continue;
                    }

                    // 이미 열린 목록에 노드가 존재한다면
                    // 상하좌우 4방향만 이동 가능하고, 모든 가중치가 동일하다면, 이미 열린 목록을 확인할 필요는 없음.
                    if (openSetDic.TryGetValue((yPos, xPos), out var neighbor) == true)
                    {
                        // 이웃한 노드를 거쳐, 현재 노드로 올 때의 Weight값 계산
                        var neighborWeight = tile[yPos, xPos].Weight;
                        var g = current < 4
                            ? curNode.G + neighborWeight
                            : curNode.G + neighborWeight * 1.4f;
                        
                        // 만약, 이웃노드->현재노드로 올 때의 값이, 이웃 노드의 g(n)보다 작다면 부모 및 g(n)값 갱신
                        if (g < neighbor.G)
                        {
                            neighbor.G = g;
                            parent[neighbor.Pos.Item1, neighbor.Pos.Item2] = curNode.Pos;
                            
                            // 갱신된 이웃 노드를 다시 열린 목록에 추가
                            openSet.Enqueue(neighbor, neighbor.TotalWeight);
                            
                            // 갱신했으므로, 이후의 과정은 하지 않아도 됨.
                            continue;
                        }
                    }
                    
                    // 이웃 노드 탐색(대각 이동이라면, 기존 가중치 값에 1.4를 곱함)
                    var weight = current < 4
                        ? tile[yPos, xPos].Weight
                        : tile[yPos, xPos].Weight * 1.4f;

                    var h = CalculateHeuristic((yPos, xPos), end);
                    var newNode = new NodeData((yPos, xPos), weight, h);
                    
                    // 부모 노드 갱신
                    parent[yPos, xPos] = curNode.Pos;
                    
                    if (openSetDic.TryAdd((yPos, xPos), newNode) == true)
                    {
                        openSet.Enqueue(newNode, newNode.TotalWeight);
                    };
                }
            }
            
            return null;
        }

        private float CalculateHeuristic((int, int) start, (int, int) end)
        {
            var dy = Mathf.Abs(start.Item1 - end.Item1);
            var dx = Mathf.Abs(start.Item2 - end.Item2);

            return (dx + dy) / 2f;
        }
    }
    
    public class NodeData : IComparable<NodeData>
    {
        public float G;
        public float H;
        public (int, int) Pos;
        public float TotalWeight => G + H;

        public NodeData((int, int) pos, float g, float h)
        {
            Pos = pos;
            G = g;
            H = h;
        }

        public int CompareTo(NodeData other)
        {
            var compare = TotalWeight.CompareTo(other.TotalWeight);

            if (compare == 0)
            {
                compare = Pos.CompareTo(other.Pos);
            }

            return compare;
        }
    }

    public static class FindPath
    {
        public static event Action<(int, int), Color> ChangeTileColor;
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
}
