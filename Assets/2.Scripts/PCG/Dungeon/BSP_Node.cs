using System.Collections.Generic;
using UnityEngine;

public abstract class BSP_Node
{
    public BSP_Node ParentNode;
    public List<BSP_Node> ChildNode;
    public bool IsVisited;

    public NodePosition Pos;
    public int Index;

    public BSP_Node(BSP_Node parent)
    {
        ParentNode = parent;
        ChildNode = new List<BSP_Node>();
        
        ParentNode?.AddChildNode(this);
    }

    public void AddChildNode(BSP_Node node)
    {
        ChildNode?.Add(node);
    }
}
