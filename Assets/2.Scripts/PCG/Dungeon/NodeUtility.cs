using UnityEngine;
using System.Collections.Generic;

public static class NodeUtility
{
    public static List<RoomNode> GetAllLeafNode(RoomNode root)
    {
        var leafNodes = new List<RoomNode>();
        var queue = new Queue<RoomNode>(new[] { root });

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
