using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorGenerator
{
    private List<RoomNode> _roomNodes;
    private DungeonData _data;

    public List<CorridorNode> GenerateCorridor(List<RoomNode> rooms, DungeonData data)
    {
        var sortedRoom = rooms.OrderByDescending(x => x.Index).ToList();
        var corridorList = new List<CorridorNode>();

        foreach (var node in sortedRoom)
        {
            if (node.ChildNode.Count == 0)
            {
                continue;
            }

            var corridor = new CorridorNode((RoomNode)node.ChildNode[0], (RoomNode)node.ChildNode[1],
                data.CorridorWidth, data.DistanceFromWall);
            
            corridorList.Add(corridor);
        }
        
        return corridorList;
    }
}
