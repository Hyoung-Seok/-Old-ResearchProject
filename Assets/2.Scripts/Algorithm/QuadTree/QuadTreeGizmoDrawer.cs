using UnityEngine;

public class QuadTreeGizmoDrawer : MonoBehaviour
{
    [SerializeField] private bool isDrawLine = true;
    [SerializeField] private QuadTreeManager manager;
    [SerializeField] private float y = 10f;
    [SerializeField] private Color nodeColor = Color.blue;

    private void OnDrawGizmos()
    {
        if (isDrawLine == false) return;
        
        if (manager == null) manager = GetComponent<QuadTreeManager>();
        if (manager == null || manager.RootNode == null) return;

        DrawNode(manager.RootNode);
    }

    private void DrawNode(QNode node)
    {
        var lb = node.LeftBottom;
        var rt = node.RightTop;

        var leftBottom = new Vector3(lb.x, y, lb.y);
        var leftTop    = new Vector3(lb.x, y, rt.y);
        var rightBottom= new Vector3(rt.x, y, lb.y);
        var rightTop   = new Vector3(rt.x, y, rt.y);

        Gizmos.color =  nodeColor;
        Gizmos.DrawLine(leftBottom, rightBottom);
        Gizmos.DrawLine(rightBottom, rightTop);
        Gizmos.DrawLine(rightTop, leftTop);
        Gizmos.DrawLine(leftTop, leftBottom);

        if (node.ChildNodes == null) return;
        
        for (var i = 0; i < node.ChildNodes.Length; ++i)
        {
            var child = node.ChildNodes[i];
            if (child != null)
                DrawNode(child);
        }
    }
}