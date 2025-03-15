using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GraphGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private GameObject nodeObj;
    [SerializeField] private GraphData graphData;

    [Header("Line Setting")] 
    [SerializeField] private Material lineMat;
    [SerializeField] private float lineWidth = 0.2f;

    [Header("Offset")] 
    [SerializeField] private int xOffset;
    [SerializeField] private int zOffset;
    [SerializeField] private float depthInterval = 1;

    public void GenerateGraph()
    {
        var graph = Graph.ArrayGraph;
        var nodeDepths = new Dictionary<int, int>() { { 0, 0 } };
        
        CreateNodes();
        
        for (var col = 0; col < graph.GetLength(0); ++col)
        {     
            var childList = new List<Node>();
            
            for (var row = 0; row < graph.GetLength(1); ++row)
            {
                if(graph[col, row] != 1) continue;
                
                childList.Add(graphData.List[row].GetComponent<Node>());
                nodeDepths[row] = nodeDepths[col] + 1;
            }
            
            // 자식 노드 위치 설정
            if(childList.Count == 0) continue;

            var parentPos = graphData.List[col].transform.position;
            var depth = nodeDepths[col];
            var dynamicXOffset = xOffset / (depth + depthInterval);

            for (var i = 0; i < childList.Count; ++i)
            {
                var xPos = parentPos.x + (i - (childList.Count - 1) / 2f) * dynamicXOffset;
                graphData.List[childList[i].Index].
                    GetComponent<Node>().SetPosition(new Vector3(xPos, 0, parentPos.z - zOffset));
            }

            graphData.List[col].GetComponent<Node>()
                .LinkLineRenderer(childList, lineMat, lineWidth);
        }
    }

    public void DestroyGraph()
    {
        for (var i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(graphData.List[i]);
        }
        
        graphData.List.Clear();
    }

    private void CreateNodes()
    {
        graphData.List = new List<GameObject>();

        for (var i = 0; i < Graph.ArrayGraph.GetLength(0); ++i)
        {
            var node = Instantiate(nodeObj, transform);
            node.GetComponent<Node>().SetIndexNumber(i);
            
            graphData.List.Add(node);
        }
    }
}
