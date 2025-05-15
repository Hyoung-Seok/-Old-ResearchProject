using UnityEngine;

public class NodePosition
{
    public Vector2Int TL { get; private set; }
    public Vector2Int TR { get; private set; }
    public Vector2Int BL { get; private set; }
    public Vector2Int BR { get; private set; }

    public NodePosition(Vector2Int bottomLeft, Vector2Int topRight)
    {
        BL = bottomLeft;
        TR = topRight;
        BR = new Vector2Int(TR.x, BL.y);
        TL = new Vector2Int(BL.x, TR.y);
    }

    public Vector2Int[] GetPositionArray()
    {
        return new[] { BL, TL, TR, BR };
    }
}
