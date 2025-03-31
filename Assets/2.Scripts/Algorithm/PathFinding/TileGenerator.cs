using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PathFinding;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum EBiome
{
    WALL = 0,
    ROAD = 1,
    SAND = 4,
    DUST = 8,
    TRAP = 10
}

public class TileGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private GameObject[] tileObject;
    
    [Header("Setting")] 
    [SerializeField] private bool isCreateBiome = false;
    [SerializeField] private int biomeRange = 8;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float offset = 0.5f;
    [SerializeField] private int delayTime = 500;

    private bool _isSkip = false;
    private readonly int[] _dx = new[] { 0, 0, -2, 2 };
    private readonly int[] _dy = new[] { -2, 2, 0, 0 };
    private Tile[,] _tile;
    private Camera _mainCam;

    private (int, int) _startPos = (-1, -1);
    private (int, int) _endPos = (-1, -1);
    private (Material, Color) _prevStartTile;
    private (Material, Color) _prevEndTile;

    private BFS _bfs;
    private Dijkstra _dijkstra;
    private AStar _aStar;
    private List<(int, int)> _path;
    
    private void Start()
    {
        if (width % 2 == 0) width -= 1;
        if (height % 2 == 0) height -= 1;

        _mainCam = Camera.main;
        
        _bfs = new BFS();
        _dijkstra = new Dijkstra();
        _aStar = new AStar();
        _path = new List<(int, int)>();

        FindPath.ChangeTileColor += ChangeTileColor;
        
        GenerateTile(SetTileArray());
    }

    public async void StartPathFinding(int type)
    {
        if (_startPos.Item1 == -1 || _endPos.Item1 == -1)
        {
            return;
        }
        
        _isSkip = false;

        switch (type)
        {
            case (int)EFindingType.BFS:
                Debug.Log("Start BFS PathFinding");
                _path = await _bfs.BFS_PathFinding(_tile, _startPos, _endPos);
                break;
            
            // case (int)EFindingType.Dijkstra:
            //     Debug.Log("Start Dijkstra PathFinding");
            //     _path = await _dijkstra.Dijkstra_PathFinding(_tile, _startPos, _endPos);
            //     break;
            //
            // case (int)EFindingType.AStar:
            //     Debug.Log("Start A* PathFinding");
            //     _path = await _aStar.AStar_PathFinding(_tile, _startPos, _endPos);
            //     break;
            
            default:
                return;
        }

        if (_path.Count == 0) return;
        
        ChangePathTileColor(_path);
    }
    
    #region Event
    
    public void SkipButtonClickEvent()
    {
        _isSkip = true;
    }

    public void OnMouseLeftButtonClickEvent()
    {
        var ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var obj = hit.collider.gameObject;
            _startPos = ConvertChildIndexToCoordinate(obj.transform.GetSiblingIndex());

            if (_prevStartTile.Item1 != null)
            {
                _prevStartTile.Item1.color = _prevStartTile.Item2;
            }

            _prevStartTile.Item1 = _tile[_startPos.Item1, _startPos.Item2].Mat;
            _prevStartTile.Item2 = _tile[_startPos.Item1, _startPos.Item2].Mat.color;
            
            _tile[_startPos.Item1, _startPos.Item2].Mat.color = Color.red;
        }
    }

    public void OnMouseRightButtonClickEvent()
    {
        var ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var obj = hit.collider.gameObject;
            _endPos = ConvertChildIndexToCoordinate(obj.transform.GetSiblingIndex());

            if (_prevEndTile.Item1 != null)
            {
                _prevEndTile.Item1.color = _prevEndTile.Item2;
            }

            _prevEndTile.Item1 = _tile[_endPos.Item1, _endPos.Item2].Mat;
            _prevEndTile.Item2 = _tile[_endPos.Item1, _endPos.Item2].Mat.color;
            
            _tile[_endPos.Item1, _endPos.Item2].Mat.color = Color.blue;
        }
    }
    
    #endregion
    
    public void ResetTileColor()
    {
        for (var col = 0; col < height; ++col)
        {
            for (var row = 0; row < width; ++row)
            {
                if(_tile[col, row].Mat.color == Color.black || _tile[col, row].Mat.color == Color.white) continue;

                _tile[col, row].Mat.color = (_tile[col, row].Weight == 1) ? Color.white : Color.black;
            }
        }

        _tile[_startPos.Item1, _startPos.Item2].Mat.color = Color.red; 
        _tile[_endPos.Item1, _endPos.Item2].Mat.color = Color.blue; 
    }

    private async void ChangePathTileColor(List<(int, int)> path)
    {
        for (var i = 1; i < path.Count - 1; ++i)
        {
            _tile[path[i].Item1, path[i].Item2].Mat.color = Color.magenta;
            
            if (_isSkip == false)
            {
                await UniTask.Delay(delayTime);
            }
        }
    }

    private async void ChangeTileColor((int, int) pos, Color color)
    {
        _tile[pos.Item1, pos.Item2].Mat.color = color;
        await UniTask.Delay(delayTime / 2);
    }

    #region GenerateMaze
    private void GenerateTile(int[,] tile)
    {
        var pos = Vector3.zero;
        _tile = new Tile[height, width];
        
        for (var col = 0; col < height; ++col)
        {
            for (var row = 0; row < width; ++row)
            {
                GameObject obj;
                
                switch (tile[col, row])
                {
                    case (int)EBiome.WALL:
                        obj = Instantiate(tileObject[0], transform);
                        break;
                    
                    case (int)EBiome.ROAD:
                        obj = Instantiate(tileObject[1], transform);
                        break;
                    
                    case (int)EBiome.SAND:
                        obj = Instantiate(tileObject[2], transform);
                        break;
                    
                    case (int)EBiome.DUST:
                        obj = Instantiate(tileObject[3], transform);
                        break;
                    
                    case (int)EBiome.TRAP:
                        obj = Instantiate(tileObject[4], transform);
                        break;
                    
                    default:
                        continue;
                }

                obj.transform.position = pos;
                pos.x += offset;

                _tile[col, row] = new Tile(obj, tile[col, row]);
            }

            pos.x = 0;
            pos.z -= offset;
        }
    }

    private int[,] SetTileArray()
    {
        var tile = new int[height, width];
        var stack = new Stack<(int, int)>();
        
        stack.Push(GetRandomPosition());

        while (stack.Count > 0)
        {
            var pos = stack.Pop();
            tile[pos.Item1, pos.Item2] = 1;

            var nextPos = GetNextPosition(tile, pos);

            if (nextPos.Item1 == -1)
            {
                continue;
            }
            
            stack.Push(pos);
            stack.Push(nextPos);
        }
        
        return (isCreateBiome == true) ? CreateRandomBiome(tile) : tile;
    }

    private int[,] CreateRandomBiome(int[,] tile)
    {
        var randX = width / 2 + Random.Range(-biomeRange, biomeRange);
        var randY = height / 2 + Random.Range(-biomeRange, biomeRange);
            
        var landBiome = new[] { EBiome.SAND, EBiome.DUST, EBiome.TRAP };
        landBiome = landBiome.OrderBy(_ => Random.value).ToArray();

        var quadrants = new[] { 0, 1, 2, 3 }.OrderBy(_ => Random.value).ToList();

        var biomeDic = new Dictionary<int, EBiome>();
        for (var i = 0; i < landBiome.Length; ++i)
        {
            biomeDic[quadrants[i]] = landBiome[i];
        }

        for (var col = 0; col < height; ++col)
        {
            for (var row = 0; row < width; ++row)
            {
                if(tile[col, row] == (int)EBiome.WALL) continue;
                    
                int q;
                if (col <= randY && row <= randX) q = 0;
                else if (col <= randY && row > randX) q = 1;
                else if (col > randY && row <= randX) q = 2;
                else q = 3;

                if (biomeDic.ContainsKey(q) == true)
                {
                    tile[col, row] = (int)biomeDic[q];
                }
            }
        }

        return tile;
    }

    private (int, int) GetNextPosition(int[,] tile, (int, int) pos)
    {
        var land = new[] { 0, 1, 2, 3 };
        land = land.OrderBy(_ => Random.value).ToArray();

        foreach (var index in land)
        {
            if (pos.Item1 + _dy[index] < 1 || pos.Item1 + _dy[index] >= height - 1 ||
                pos.Item2 + _dx[index] < 1 || pos.Item2 + _dx[index] >= width - 1 ||
                tile[pos.Item1 + _dy[index], pos.Item2 + _dx[index]] == 1)
            {
                continue;
            }

            tile[pos.Item1 + _dy[index] / 2, pos.Item2 + _dx[index] / 2] = 1;
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

    #endregion
    
    private (int, int) ConvertChildIndexToCoordinate(int index)
    {
        var yPos = index / width;
        var xPos = index % width;

        return (yPos, xPos);
    }
}
