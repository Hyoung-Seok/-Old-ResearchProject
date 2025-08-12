using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QNode : MonoBehaviour
{
    public Vector2 LeftBottom { get; private set; }
    public Vector2 RightTop { get; private set; }
    
    public List<int> IncludeObjectIndex = new List<int>();
    public QNode[] ChildNodes;

    [SerializeField] private List<EnemyObject> includeObject;

    public void SetPosition(Vector2 lb, Vector2 rt)
    {
        LeftBottom = lb;
        RightTop = rt;
    }

    public void UpdateIncludeObject(List<EnemyObject> objList)
    {
        includeObject = new List<EnemyObject>();

        for (var i = 0; i < IncludeObjectIndex.Count; ++i)
        {
            includeObject.Add(objList[IncludeObjectIndex[i]]);
        }
    }
}
