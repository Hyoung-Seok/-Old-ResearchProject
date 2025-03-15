using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GraphGenerator))]
public class GraphInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var graphGenerator = (GraphGenerator)target;

        if (Application.isPlaying == true)
        {
            return;
        }
        
        GUILayout.Space(10f);
        if (GUILayout.Button("Generate Graph"))
        {
            graphGenerator.GenerateGraph();
        }

        GUILayout.Space(10f);

        if (GUILayout.Button("Destroy Graph"))
        {
            graphGenerator.DestroyGraph();
        }
    }
}
