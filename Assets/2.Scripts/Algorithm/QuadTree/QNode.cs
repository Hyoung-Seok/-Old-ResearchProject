using System.Collections.Generic;
using UnityEngine;

public class QNode : MonoBehaviour
{
    public Vector2 LeftBottom { get; private set; }
    public Vector2 RightTop { get; private set; }

    public List<int> IncludeObjectIndex = new List<int>();
    
    public QNode[] ChildNodes;

    public void SetPosition(Vector2 lb, Vector2 rt)
    {
        LeftBottom = lb;
        RightTop = rt;
    }
}
