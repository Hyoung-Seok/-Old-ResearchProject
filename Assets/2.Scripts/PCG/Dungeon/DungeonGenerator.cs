using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private LineDisplay lineDisplay;

    [SerializeField] private MeshCreater meshCreater;
    
    [Header("Create Setting")] 
    [SerializeField] private DungeonData dungeonData;
    
    private List<RoomNode> _roomNodeList;
    private List<RoomNode> _leafNode;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        var bsp = new BinarySpacePartitioning();
        var roomGenerator = new RoomGenerator();
        
        _roomNodeList = bsp.BSP(dungeonData);
        _leafNode = GetAllLeafNode();
        
        roomGenerator.GenerateRoom(_leafNode, dungeonData);
        lineDisplay.DisplayLine(_roomNodeList[0]);
        lineDisplay.DisplayLine(_leafNode);
        
        _leafNode.ForEach(x => meshCreater.CreateMesh(x.RoomPosition));
    }

    private List<RoomNode> GetAllLeafNode()
    {
        var leafNodes = new List<RoomNode>();
        var queue = new Queue<RoomNode>(new[] { _roomNodeList[0] });

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.ChildNode.Count <= 0)
            {
                leafNodes.Add(node);
                continue;
            }

            foreach (var chile in node.ChildNode)
            {
                queue.Enqueue((RoomNode)chile);
            }
        }

        return leafNodes;
    }
}
