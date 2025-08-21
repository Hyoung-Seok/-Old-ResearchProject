using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QNode : MonoBehaviour
{
    [SerializeField] private Vector2 leftBottom;
    [SerializeField] private Vector2 rightTop;

    public Vector2 LeftBottom => leftBottom;
    public Vector2 RightTop => rightTop;

    public List<int> IncludeObjectIndex = new List<int>();
    public QNode[] ChildNodes = null;

    private List<IQuadObject> _includeObject;

    public void SetPosition(Vector2 lb, Vector2 rt)
    {
        leftBottom = lb;
        rightTop = rt;
    }

    public void SetStateIncludeObject(bool isEnable)
    {
        foreach (var obj in _includeObject)
        {
            if (isEnable == true) obj.EnableObject();
            else obj.DisableObject();
        }
    }

    public void CacheObject(List<IQuadObject> objList)
    {
        _includeObject = new List<IQuadObject>();
        IncludeObjectIndex.ForEach(i => _includeObject.Add(objList[i]));
    }
}
