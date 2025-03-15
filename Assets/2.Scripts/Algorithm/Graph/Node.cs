using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private TextMeshPro index;
    [SerializeField] private Transform[] posTf;
    
    public int Index => int.Parse(index.text);

    public void SetIndexNumber(int num)
    {
        index.text = num.ToString();
    }

    public void LinkLineRenderer(List<Node> linkNode, Material mat, float width)
    {
        foreach (var node in linkNode)
        {
            var lineObj = new GameObject("LineObj");
            lineObj.transform.SetParent(posTf[1]);
            
            var lineRenderer = lineObj.AddComponent<LineRenderer>();

            lineRenderer.startWidth = lineRenderer.endWidth = width;
            lineRenderer.material = mat;
            lineRenderer.positionCount = 2;
            
            lineRenderer.SetPosition(0, posTf[1].position);
            lineRenderer.SetPosition(1, node.posTf[0].position);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
