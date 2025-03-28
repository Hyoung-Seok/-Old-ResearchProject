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

    public class AStar
    {
        public event Action<(int, int), Color> ChangeTileColor; 
        
        private const int WEIGHT = 10;
        private const int DIAGONAL_WEIGHT = 14;
        
        private readonly int[] _dx = { 0, 0, -1, 1, -1, -1, 1, 1 };
        private readonly int[] _dy = { -1, 1, 0, 0, -1, 1, -1, 1 };

        public async UniTask<List<(int, int)>> AStar_PathFinding(int[,] tile, (int, int) start, (int, int) end)
        {
            var height = tile.GetLength(0);
            var width = tile.GetLength(1);

            var parent = new (int, int)[height, width];
            var visited = new bool[height, width];
            var openNode = new PriorityQueue<NodeData, int>();
            var nodeDic = new Dictionary<(int, int), NodeData>();

            var nodeData = new NodeData(start, 0, 0);
            openNode.Enqueue(nodeData, 0);
            nodeDic.Add(nodeData.Pos, nodeData);
            
            while (openNode.Count > 0)
            {
                openNode.TryDequeue(out var curNode, out var f);

                if (curNode.Pos == end)
                {
                    return FindPath.FindShortPath(parent, start, end);
                }

                var cord = curNode.Pos;
                visited[cord.Item1, cord.Item2] = true;

                // 선택된 노드
                ChangeTileColor?.Invoke(cord, Color.red);
                await UniTask.Delay(30);
                
                // 노드 탐색 시작
                for (var i = 0; i < _dx.Length; ++i)
                {
                    var xPos = cord.Item2 + _dx[i];
                    var yPos = cord.Item1 + _dy[i];
                    
                    // 이동 가능한 타일인지 탐색
                    if(xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) continue;
                    if(visited[yPos,xPos] == true || tile[yPos, xPos] == 0) continue;

                    NodeData newNode;
                    
                    // 이미 노드가 존재한다면
                    if (nodeDic.TryGetValue((yPos, xPos), out var neighbor) == true)
                    {
                        // 인접한 노드가 상하좌우라면 10, 대각선이라면 14를 기존 g(n)에 더함
                        var g = (i < 4) ? neighbor.G + WEIGHT : neighbor.G + DIAGONAL_WEIGHT;
                        
                        // 현재 노드를 지나쳐 갈 경우의 g(n)값이 더 낮다면
                        if (g < neighbor.G)
                        {
                            // g값하고 h값만 갱신하면 된다?
                            neighbor.G = g;
                            
                            // 인접 노드의 부모 노드를 현재 노드로 갱신
                            parent[neighbor.Pos.Item1, neighbor.Pos.Item2] = cord;
                            openNode.Enqueue(neighbor, neighbor.GetTotalWeight());
                            
                            // 이미 노드가 존재한다면, 아래의 코드는 실행할 필요 없음
                            continue;
                        }
                    }
                    
                    // 상하좌우인 경우 가중치 값 갱신
                    var h = CalculateHeuristic((xPos, yPos), end);;
                    
                    if (i < 4)
                    {
                        newNode = new NodeData((yPos, xPos), curNode.G + WEIGHT, h);
                    }
                    // 대각선인 경우 가중치 값 계산
                    else
                    {
                        // 이동 불가능한 경우인지 확인
                        if( 0 > yPos + _dy[i] || yPos + _dy[i] <= height || 0 > xPos + _dx[i] || xPos + _dx[i] <= width) continue;
                        if (tile[yPos + _dy[i], xPos] == 0 || tile[yPos, xPos + _dx[i]] == 0) continue;
                        
                        newNode = new NodeData((yPos, xPos), curNode.G + DIAGONAL_WEIGHT, h);
                    }

                    parent[yPos, xPos] = cord;
                    openNode.Enqueue(newNode, newNode.GetTotalWeight());
                    nodeDic.Add(newNode.Pos, newNode);
                    
                    // 탐색된 노드
                    ChangeTileColor?.Invoke(cord, Color.gray);
                    await UniTask.Delay(30);
                }
            }

            return null;
        }

        private int CalculateHeuristic((int, int) start, (int, int) end)
        {
            var dy = Mathf.Abs(start.Item1 - end.Item1);
            var dx = Mathf.Abs(start.Item2 - end.Item2);

            return (dx + dy) / 2;
        }
    }

    public class NodeData
    {
        public int G;
        public int H;
        public (int, int) Pos;

        public NodeData((int, int) pos, int g, int h)
        {
            Pos = pos;
            G = g;
            H = h;
        }

        public int GetTotalWeight()
        {
            return G + H;
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
