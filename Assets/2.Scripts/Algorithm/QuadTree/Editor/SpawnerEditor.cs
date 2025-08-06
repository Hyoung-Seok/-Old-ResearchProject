using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectRandomSpawner))]
public class SpawnerEditor : Editor
{
   private ObjectRandomSpawner _spawner;
   private void OnEnable()
   {
      _spawner = (ObjectRandomSpawner)target;
   }

   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();

      GUILayout.Space(5);
      if (GUILayout.Button("Spawn Object"))
      {
         _spawner.SpawnObjects();
      }
      
      GUILayout.Space(5);
      if (GUILayout.Button("Delete Object"))
      {
         _spawner.DeleteObjects();
      }
   }
}
