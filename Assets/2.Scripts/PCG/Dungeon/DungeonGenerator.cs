using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Create Setting")] 
    [SerializeField] private DungeonData dungeonData;
    
    private List<RoomNode> _roomNodeList;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        var bsp = new BinarySpacePartitioning();
        var roomGenerator = new RoomGenerator();
        var lineDisplay = GetComponent<LineDisplay>();
        
        _roomNodeList = bsp.BSP(dungeonData);
        
        Debug.Log(GetAllLeafNode().Count);
        roomGenerator.GenerateRoom(GetAllLeafNode(), dungeonData);
        lineDisplay.DisplayLine(_roomNodeList[0]);
        lineDisplay.DisplayLine(GetAllLeafNode());
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
