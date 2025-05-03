using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Create Setting")] 
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int roomMinWidth;
    [SerializeField] private int roomMinHeight;
    [SerializeField] private int iteration;
    
    private List<RoomNode> _roomNodeList;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        _roomNodeList = new List<RoomNode>();

        var rootPos = new NodePosition(new Vector2Int(0, height), new Vector2Int(width, 0));
        var root = new RoomNode(null, rootPos, 0);

        _roomNodeList = SplitSpace(root);
        GetComponent<LineDisplay>().DisplayLine(_roomNodeList);
    }

    private List<RoomNode> SplitSpace(RoomNode root)
    {
        var graph = new Queue<RoomNode>();
        var result = new List<RoomNode>();
        var iter = 0;
        
        graph.Enqueue(root);
        result.Add(root);

        while (iter < iteration && graph.Count > 0)
        {
            iter++;

            var curNode = graph.Dequeue();
            SplitSpace(curNode, graph, result);
        }

        return result;
    }

    private void SplitSpace(RoomNode node, Queue<RoomNode> graph, List<RoomNode> nodeList)
    {
        // 1. 가로, 세로 분할 여부 확인
        var widthStatus = node.Width >= roomMinWidth * 2;
        var heightStatus = node.Height >= roomMinHeight * 2;
        
        var splitDir = (widthStatus, heightStatus) switch
        {
            (true, true) => (ELine)Random.Range(0, 2),
            (true, false) => ELine.Vertical,
            (false, true) => ELine.Horizontal,
            _ => ELine.None,
        };

        RoomNode node1 = null;
        RoomNode node2 = null;
        NodePosition pos1 = null;
        NodePosition pos2 = null;
        var newPos = 0;
        
        switch (splitDir)
        {
            case ELine.None:
                return;
            
            case ELine.Vertical:
                newPos = Random.Range(node.Pos.BL.x + roomMinWidth, node.Pos.BR.x - roomMinWidth);

                pos1 = new NodePosition(node.Pos.TL, new Vector2Int(newPos, node.Pos.BL.y));
                pos2 = new NodePosition(new Vector2Int(newPos, node.Pos.TL.y), node.Pos.BR);
                break;
            
            case ELine.Horizontal:
                newPos = Random.Range(node.Pos.BL.y + roomMinHeight, node.Pos.TL.y - roomMinHeight);
                
                pos1 = new NodePosition(node.Pos.TL, new Vector2Int(node.Pos.TR.x, newPos));
                pos2 = new NodePosition(new Vector2Int(node.Pos.TL.x, newPos), node.Pos.BR);
                break;
        }

        node1 = new RoomNode(node, pos1, node.Index + 1);
        node2 = new RoomNode(node, pos2, node.Index + 1);
        
        nodeList.Add(node1);
        graph.Enqueue(node1);

        nodeList.Add(node2);
        graph.Enqueue(node2);
    }
}

public enum ELine
{
    None = -1,
    Horizontal = 0,
    Vertical = 1
}
