using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphSearch : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private GameObject cursorObj;
    [SerializeField] private GraphData graphData;

    [Header("UI")] 
    [SerializeField] private TMP_InputField startNode;

    private int[,] _arrayGraph;
    private List<List<int>> _listGraph;
    private bool[] _visited;

    private void Start()
    {
        _arrayGraph = Graph.ArrayGraph;
        _listGraph = Graph.ListGraph;
    }

    #region DFS
    
    public async void DFS_Array()
    {
        var stack = new Stack<int>();

        var startIndex = ResetAndGetStartIndex(_arrayGraph.GetLength(0));
        stack.Push(startIndex);
        _visited[startIndex] = true;

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            cursorObj.transform.position = graphData.List[node].transform.position;

            await UniTask.Delay(800);

            for (var i = _arrayGraph.GetLength(1) - 1; i >= 0; --i)
            {
                if(_arrayGraph[node, i] != 1 || _visited[i] == true) continue;
                
                _visited[i] = true;
                stack.Push(i);
            }
        }
    }

    public async void DFS_List()
    {
        var stack = new Stack<int>();
        
        var startIndex = ResetAndGetStartIndex(_listGraph.Count);
        stack.Push(startIndex);
        _visited[startIndex] = true;

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            cursorObj.transform.position = graphData.List[node].transform.position;
            
            await UniTask.Delay(800);

            for (var i = _listGraph[node].Count - 1; i >= 0; --i)
            {
                if (_visited[_listGraph[node][i]] == true) continue;
                
                _visited[_listGraph[node][i]] = true;
                stack.Push(_listGraph[node][i]);
            }
        }
    }
    
    #endregion

    #region BFS

    public async void BFS_Array()
    {
        var queue = new Queue<int>();

        var startIndex = ResetAndGetStartIndex(_arrayGraph.GetLength(0));
        queue.Enqueue(startIndex);
        _visited[startIndex] = true;
        
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            cursorObj.transform.position = graphData.List[node].transform.position;

            await UniTask.Delay(800);

            for (var i = 0; i < _arrayGraph.GetLength(1); ++i)
            {
                if(_visited[i] == true || _arrayGraph[node, i] != 1) continue;

                _visited[i] = true;
                queue.Enqueue(i);
            }
        }
    }
    
    public async void BFS_List()
    {
        var queue = new Queue<int>();

        var startIndex = ResetAndGetStartIndex(_listGraph.Count);
        queue.Enqueue(startIndex);
        _visited[startIndex] = true;
        
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            cursorObj.transform.position = graphData.List[node].transform.position;

            await UniTask.Delay(800);

            foreach (var next in _listGraph[node])
            {
                _visited[next] = true;
                queue.Enqueue(next);
            }
        }
    }

    #endregion

    private int ResetAndGetStartIndex(int length)
    {
        _visited = new bool[length];
        cursorObj.transform.position = graphData.List[0].transform.position;

        if (int.TryParse(startNode.text, out var startIndex) == false)
        {
            startIndex = 0;
        }

        return startIndex;
    }
}
