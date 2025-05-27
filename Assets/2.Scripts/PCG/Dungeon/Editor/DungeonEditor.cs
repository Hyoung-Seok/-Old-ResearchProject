using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var dungeonGenerator = (DungeonGenerator)target;

        if (GUILayout.Button("Generate Dungeon"))
        {
            dungeonGenerator.GenerateDungeon();
        }

        if (GUILayout.Button("Reset"))
        {
            dungeonGenerator.ResetDungeon();
        }
    }
    
}
