using System.Collections.Generic;
using UnityEngine;

public enum ELine
{
    None = -1,
    Horizontal = 0,
    Vertical = 1
}

public class BinarySpacePartitioning
{
    private DungeonData _data;

    public List<RoomNode> BSP(DungeonData data)
    {
        _data = data;

        var rootPos = new NodePosition(new Vector2Int(0, _data.Height), 
            new Vector2Int(_data.Width, 0));
        var rootNode = new RoomNode(null, rootPos, 0);

        var result = new List<RoomNode> { rootNode };
        var graph = new Queue<RoomNode>(new[] { rootNode });
        var iter = 0;

        while (iter < _data.Iteration && graph.Count > 0)
        {
            iter++;

            var curNode = graph.Dequeue();
            SplitSpace(curNode, graph, result);
        }

        return result;
    }

    private void SplitSpace(RoomNode node, Queue<RoomNode> graph, List<RoomNode> nodeList)
    {
        var widthStatus = node.Width >= _data.RoomMinWidth * 2;
        var heightStatus = node.Height >= _data.RoomMinHeight * 2;

        var splitDir = (widthStatus, heightStatus) switch
        {
            (true, true) => (ELine)Random.Range(0,2),
            (false, true) => ELine.Horizontal,
            (true, false) => ELine.Vertical,
            _ => ELine.None
        };

        var curPos = node.Pos;
        var newPos = 0;
        NodePosition pos1 = null;
        NodePosition pos2 = null;

        switch (splitDir)
        {
            case ELine.Horizontal:
                newPos = Random.Range(curPos.BL.y + _data.RoomMinHeight,
                    curPos.TL.y - _data.RoomMinHeight);

                pos1 = new NodePosition(curPos.TL, new Vector2Int(curPos.BR.x, newPos));
                pos2 = new NodePosition(new Vector2Int(curPos.BL.x, newPos), curPos.BR);
                break;

            case ELine.Vertical:
                newPos = Random.Range(curPos.BL.x + _data.RoomMinWidth,
                    curPos.BR.x - _data.RoomMinWidth);

                pos1 = new NodePosition(curPos.TL, new Vector2Int(newPos, curPos.BL.y));
                pos2 = new NodePosition(new Vector2Int(newPos, curPos.TL.y), curPos.BR);
                break;

            case ELine.None:
            default:
                return;
        }

        var node1 = new RoomNode(node, pos1, node.Index + 1);
        var node2 = new RoomNode(node, pos2, node.Index + 1);
        
        graph.Enqueue(node1);
        nodeList.Add(node1);

        graph.Enqueue(node2);
        nodeList.Add(node2);
    }
}
