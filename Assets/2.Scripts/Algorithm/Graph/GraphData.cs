using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphData", menuName = "Scriptable Objects/GraphData")]
public class GraphData : ScriptableObject
{
    [HideInInspector] public List<GameObject> List;
}

