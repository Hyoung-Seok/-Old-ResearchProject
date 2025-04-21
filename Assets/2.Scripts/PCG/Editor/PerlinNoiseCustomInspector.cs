using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinNoiseManager))]
public class PerlinNoiseCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var noiseManager = (PerlinNoiseManager)target;

        if (GUILayout.Button("Generate") == true)
        {
            noiseManager.GeneratePerlinNoiseMap();
        }
    }
}
