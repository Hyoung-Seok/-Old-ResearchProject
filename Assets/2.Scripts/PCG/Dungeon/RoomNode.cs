using UnityEngine;

public class RoomNode : BSP_Node
{
    public int Width => Mathf.Abs(Pos.BR.x - Pos.TL.x);
    public int Height => Mathf.Abs(Pos.TL.y - Pos.BL.y);
    public NodePosition RoomPosition { get; private set; }
        
    public RoomNode(BSP_Node parent, NodePosition position, int index) : base(parent)
    {
        Pos = position;
        Index = index;
        RoomPosition = null;
    }

    public void AddRoomPosition(NodePosition position)
    {
        RoomPosition = position;
    }
}
