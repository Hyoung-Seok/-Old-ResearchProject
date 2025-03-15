using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private GameObject nodeObj;

    [Header("Line Setting")] 
    [SerializeField] private Material lineMat;
    [SerializeField] private float lineWidth = 0.2f;

    [Header("Offset")] 
    [SerializeField] private int xOffset;
    [SerializeField] private int zOffset;
    [SerializeField] private float depthInterval = 1;

    private List<Node> _nodeList;

    private void Start()
    {
        GenerateGraph();
    }

    private void GenerateGraph()
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
                
                childList.Add(_nodeList[row]);
                nodeDepths[row] = nodeDepths[col] + 1;
            }
            
            // 자식 노드 위치 설정
            if(childList.Count == 0) continue;

            var parentPos = _nodeList[col].transform.position;
            var depth = nodeDepths[col];
            var dynamicXOffset = xOffset / (depth + depthInterval);

            for (var i = 0; i < childList.Count; ++i)
            {
                var xPos = parentPos.x + (i - (childList.Count - 1) / 2f) * dynamicXOffset;
                _nodeList[childList[i].Index].SetPosition(new Vector3(xPos, 0, parentPos.z - zOffset));
            }
            
            _nodeList[col].LinkLineRenderer(childList, lineMat, lineWidth);
        }
    }

    private void CreateNodes()
    {
        _nodeList = new List<Node>();

        for (var i = 0; i < Graph.ArrayGraph.GetLength(0); ++i)
        {
            var node = Instantiate(nodeObj).GetComponent<Node>();
            node.transform.SetParent(transform);
            node.SetIndexNumber(i);
            
            _nodeList.Add(node);
        }
    }
}
