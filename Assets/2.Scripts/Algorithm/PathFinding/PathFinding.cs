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
        public List<(int, int)> BFS_PathFinding(Tile[,] tile, (int, int) start, (int, int) end)
        {
            PrefCheck.StartPrefCheck();
            
            var visited = new bool[tile.GetLength(0), tile.GetLength(1)];
            var queue = new Queue<(int, int)>();
            var parent = new (int, int)[tile.GetLength(0), tile.GetLength(1)];

            queue.Enqueue(start);
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                PrefCheck.AddCheckTile(node);
                
                if (node == end)
                {
                    PrefCheck.EndPrefCheck();
                    return PathUtils.FindShortPath(parent, start, end);
                }

                var cnt = -1;
                
                foreach (var dir in PathUtils.SearchDir)
                {
                    cnt++;
                    var yPos = node.Item1 + dir.y;
                    var xPos = node.Item2 + dir.x;

                    if (xPos < 0 || xPos >= tile.GetLength(1) || yPos < 0 ||
                        yPos >= tile.GetLength(0))
                    {
                        continue;
                    }

                    if (tile[yPos, xPos].Weight == 0 || visited[yPos, xPos] == true)
                    {
                        continue;
                    }

                    if (cnt >= 4 && PathUtils.IsDiagonalBlocked(tile, node, (dir.y, dir.x)) == true)
                    {
                        continue;
                    }

                    queue.Enqueue((yPos, xPos));
                    visited[yPos, xPos] = true;
                    parent[yPos, xPos] = node;
                }
            }

            PrefCheck.EndPrefCheck();
            return null;
        }
    }

    public class Dijkstra
    {
        public List<(int, int)> Dijkstra_PathFinding(Tile[,] tile, (int, int) start, (int, int) end)
        {
            PrefCheck.StartPrefCheck();
            
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
                PrefCheck.AddCheckTile(node);

                if(visited[node.Item1, node.Item2] == true) continue;
                
                if (node == end)
                {
                    PrefCheck.EndPrefCheck();
                    return PathUtils.FindShortPath(parent, start, end);
                }
                
                visited[node.Item1, node.Item2] = true;
                var cnt = -1;

                foreach (var dir in PathUtils.SearchDir)
                {
                    cnt++;
                    
                    var xPos = node.Item2 + dir.x;
                    var yPos = node.Item1 + dir.y;
                    
                    if(xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) continue;
                    if(visited[yPos, xPos] == true || tile[yPos, xPos].Weight == (int)EBiome.WALL) continue;
                    
                    // 대각선 이동 가능 검사
                    if (cnt >= 4 && PathUtils.IsDiagonalBlocked(tile, node, (yPos, xPos)) == true)
                    {
                        continue;
                    }

                    var weight = (cnt >= 4) ? tile[yPos, xPos].Weight * 1.4f : tile[yPos, xPos].Weight;
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

            PrefCheck.EndPrefCheck();
            return null;
        }
    }

    public class AStar
    {
        public List<(int, int)> AStar_PathFinding(Tile[,] tile, (int, int) start, (int, int) end)
        {
            PrefCheck.StartPrefCheck();
            
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
                if (openSet.TryDequeue(out var curNode, out var f) == false)
                {
                    PrefCheck.EndPrefCheck();
                    return null;
                }
                
                PrefCheck.AddCheckTile(curNode.Pos);
                
                if (curNode.Pos == end)
                {
                    PrefCheck.EndPrefCheck();
                    return PathUtils.FindShortPath(parent, start, end);
                }

                closeSet[curNode.Pos.Item1, curNode.Pos.Item2] = true;

                var current = -1;
                
                foreach (var dir in PathUtils.SearchDir)
                {
                    current++;
                    var xPos = curNode.Pos.Item2 + dir.x;
                    var yPos = curNode.Pos.Item1 + dir.y;
                    
                    if(xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) continue;
                    if(closeSet[yPos, xPos] == true || tile[yPos, xPos].Weight == (int)EBiome.WALL) continue;
                    
                    //  대각선 이동이 가능한지 확인
                    if (current >= 4 && PathUtils.IsDiagonalBlocked(tile, curNode.Pos, (dir.y, dir.x)) == true)
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
            
            PrefCheck.EndPrefCheck();
            return null;
        }

        // 대각선 이동이 가능하므로, 맨허튼 거리 대신 대각선 이동의 정수 근사 휴리스틱 (10/14 방식) 사용.
        private float CalculateHeuristic((int, int) start, (int, int) end)
        {
            var dx = Mathf.Abs(start.Item2 - end.Item2);
            var dy = Mathf.Abs(start.Item1 - end.Item1);

            var dir1 = 10f;
            var dir2 = 14f;

            return dir1 * (dx + dy) + (dir2 - 2 * dir1) * Mathf.Min(dx, dy);
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
}
