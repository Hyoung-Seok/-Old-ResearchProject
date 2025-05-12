using UnityEngine;

public enum ERelative
{
    Right,
    Left,
    Top,
    Bottom
}

public class CorridorNode : BSP_Node
{
    private int _width;
    private int _interval;

    public CorridorNode(RoomNode node1, RoomNode node2, int width, int distanceFromWall) : base(null)
    {
        _width = width;
        _interval = distanceFromWall;
        
        GenerateCorridor(node1, node2);
    }

    private void GenerateCorridor(RoomNode node1, RoomNode node2)
    {
        var relative = GetRelativePosition(node1, node2);

        switch (relative)
        {
            case ERelative.Right:
                break;
            
            case ERelative.Left:
                break;
            
            case ERelative.Top:
                break;
            
            case ERelative.Bottom:
                break;
            
            default:
                return;
        }
    }

    private ERelative GetRelativePosition(RoomNode node1, RoomNode node2)
    {
        var bl1 = node1.Pos.BL;
        var bl2 = node2.Pos.BL;

        var mid1 = new Vector2Int(bl1.x + node1.Width / 2, bl1.y + node1.Height / 2);
        var mid2 = new Vector2Int(bl2.x + node2.Width / 2, bl2.y + node2.Height / 2);

        var angle = Mathf.Atan2(mid2.y - mid1.y, mid2.x - mid1.x);
        
        if (45f < angle && angle < 135f) return ERelative.Top;
        else if (-135f < angle && angle < -45) return ERelative.Bottom;
        else if ((0 < angle && angle < 45f) && (-45 < angle && angle < 0)) return ERelative.Right;
        else return ERelative.Left;
    }
    
}
