using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class QuadTreeManager : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private ObjectRandomSpawner objectSpawner;
    [SerializeField] private QNode rootNode;
    
    public QNode RootNode => rootNode;
    
    public void GenerateQuadTree(int includeCount)
    {
        if(includeCount <= 0) return;
        
        var objList = objectSpawner.GetSpawnObjectsOrNull();
        if (objList == null) return;
        
        SetRootNode();
        
        var stack = new Stack<QNode>();
        stack.Push(rootNode);

        while (stack.Count > 0)
        {
            var node = stack.Pop();

            for (var i = 0; i < objList.Count; ++i)
            {
                if (CheckObjectInNode(objList[i].transform.position, node) == true)
                {
                    node.IncludeObjectIndex.Add(i);  
                }
            }
            
            if (node.IncludeObjectIndex.Count < includeCount) continue;
            
            SplitNode(node);
            for (var i = 0; i < 4; ++i)
            {
                stack.Push(node.ChildNodes[i]);
            }
        }
    }

    private void SplitNode(QNode node)
    {
        node.ChildNodes = new QNode[4];

        var lb = node.LeftBottom;
        var rt = node.RightTop;
        var mid = CalculateMidPoint(node);

        node.ChildNodes[0]= new GameObject("Node1").AddComponent<QNode>();
        node.ChildNodes[0].SetPosition(mid, rt);
        node.ChildNodes[0].transform.SetParent(node.transform);
        
        node.ChildNodes[1] = new GameObject("Node2").AddComponent<QNode>();
        node.ChildNodes[1].SetPosition(new Vector2(lb.x, mid.y), 
            new Vector2(mid.x, rt.y));
        node.ChildNodes[1].transform.SetParent(node.transform);
        
        node.ChildNodes[2] = new GameObject("Node3").AddComponent<QNode>();
        node.ChildNodes[2].SetPosition(lb, mid);
        node.ChildNodes[2].transform.SetParent(node.transform);
        
        node.ChildNodes[3] = new GameObject("Node4").AddComponent<QNode>();
        node.ChildNodes[3].SetPosition(new Vector2(mid.x, lb.y), 
            new Vector2(rt.x, mid.y));
        node.ChildNodes[3].transform.SetParent(node.transform);
    }

    private void SetRootNode()
    {
        if (rootNode != null)
        {
            DestroyImmediate(rootNode.gameObject);
        }
        
        rootNode = new GameObject("RootNode").AddComponent<QNode>();

        rootNode.transform.SetParent(transform);
        rootNode.SetPosition(ConvertVector3ToVector2(transform.Find("LeftBottom").position),
            ConvertVector3ToVector2(transform.Find("RightTop").position));
    }
    
    private bool CheckObjectInNode(Vector3 pos, QNode node)
    {
        if (pos.x < node.LeftBottom.x || pos.x > node.RightTop.x)
        {
            return false;
        }

        if (pos.z < node.LeftBottom.y || pos.z > node.RightTop.y)
        {
            return false;
        }

        return true;
    }

    private Vector2 CalculateMidPoint(QNode node)
    {
        return new Vector2((node.LeftBottom.x + node.RightTop.x) / 2,
            (node.LeftBottom.y + node.RightTop.y) / 2);
    }
    
    private Vector2 ConvertVector3ToVector2(Vector3 pos)
    {
        return new Vector2(pos.x, pos.z);
    }
}
