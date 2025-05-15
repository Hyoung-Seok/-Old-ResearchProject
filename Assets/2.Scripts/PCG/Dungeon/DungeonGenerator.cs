using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private LineDisplay lineDisplay;
    [SerializeField] private MeshGenerator meshGenerator;

    [Header("Create Setting")] 
    [SerializeField] private bool checkConditions;
    [SerializeField] private DungeonData dungeonData;
    
    private List<RoomNode> _roomNodeList;
    private List<RoomNode> _leafNode;

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        var bsp = new BinarySpacePartitioning();
        var roomGenerator = new RoomGenerator();
        var corridorGenerator = new CorridorGenerator();
        
        _roomNodeList = bsp.BSP(dungeonData, checkConditions);
        _leafNode = NodeUtility.GetAllLeafNode(_roomNodeList[0]);
        
        roomGenerator.GenerateRoom(_leafNode, dungeonData);
        lineDisplay.DisplayLine(_roomNodeList[0]);
        lineDisplay.DisplayLine(_leafNode);
        
        _leafNode.ForEach(x => meshGenerator.CreateMesh(x.RoomPosition));
        
        var corridorNode = corridorGenerator.GenerateCorridor(_roomNodeList, dungeonData);
        corridorNode.ForEach(x => meshGenerator.CreateMesh(x.Pos));
    }

    public void ResetDungeon()
    {
        for (var i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
