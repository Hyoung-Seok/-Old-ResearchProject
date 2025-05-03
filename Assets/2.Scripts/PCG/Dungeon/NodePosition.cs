using UnityEngine;

public class NodePosition
{
    public Vector2Int TL { get; private set; }
    public Vector2Int TR { get; private set; }
    public Vector2Int BL { get; private set; }
    public Vector2Int BR { get; private set; }

    public NodePosition(Vector2Int topLeft, Vector2Int bottomRight)
    {
        TL = topLeft;
        BR = bottomRight;
        TR = new Vector2Int(BR.x, TL.y);
        BL = new Vector2Int(TL.x, BR.y);
    }

    public Vector2Int[] GetPositionArray()
    {
        return new[] { BL, TL, TR, BR };
    }
}
