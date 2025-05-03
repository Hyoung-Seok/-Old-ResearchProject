using System;
using System.Collections.Generic;
using UnityEngine;

public class LineDisplay : MonoBehaviour
{
    [Header("Line Setting")] 
    [SerializeField] private float width;
    [SerializeField] private Material lineMat;

    public void DisplayLine(List<RoomNode> nodeList)
    {
        var queue = new Queue<RoomNode>();
        queue.Enqueue(nodeList[0]);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            var obj = new GameObject($"Room{node.Index}").AddComponent<LineRenderer>();
            obj.transform.SetParent(transform);
            obj.positionCount = 5;

            var posArr = node.Pos.GetPositionArray();
            for (var i = 0; i < posArr.Length +1; ++i)
            {
                var cur = posArr[i % posArr.Length];
                obj.SetPosition(i, new Vector3(cur.x, 0, cur.y));
            }

            foreach (var child in node.ChildNode)
            {
                queue.Enqueue((RoomNode)child);
            }
        }
    }

}
