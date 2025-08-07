using System;
using UnityEditor;
using UnityEngine;

public class QuadTreeEditor : EditorWindow
{
    private int _objectCount;

    private GameObject _leftBottom;
    private GameObject _rightTop;
    
    [MenuItem("Tools/QuadTree", validate = false, priority = -1)]
    private static void Init()
    {
        var window = GetWindow<QuadTreeEditor>();

        window.position = new Rect(800, 500, 500, 800);
        window.Show();
    }

    public void OnGUI()
    {
        var label = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        GUILayout.Label("Quad Tree Setting", label);
        
        GUILayout.Space(10);

        _objectCount = EditorGUILayout.IntField("객체 수 입력", _objectCount);
        
        GUILayout.Space(5);

        if (GUILayout.Button("Bake"))
        {
            FindFirstObjectByType<QuadTreeManager>().GenerateQuadTree(_objectCount);
        }
    }

    private void OnEnable()
    {
        var parent = GameObject.Find("QuadTree").transform;
        
        if (_leftBottom == null)
        {
            _leftBottom = new GameObject("LeftBottom");
            _leftBottom.transform.SetParent(parent);
        }

        if (_rightTop == null)
        {
            _rightTop = new GameObject("RightTop");
            _rightTop.transform.SetParent(parent);
        }

        SceneView.duringSceneGui += OnSceneGUI;
    }
    
    private void OnDisable()
    {
        if (_leftBottom != null)
        {
           DestroyImmediate(_leftBottom);
        }

        if (_rightTop != null)
        {
            DestroyImmediate(_rightTop);
        }

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView view)
    {
        if(_leftBottom == null || _rightTop == null) return;

        var lbT = _leftBottom.transform.position;
        var rtT = _rightTop.transform.position;

        DrawLine(new Vector2(lbT.x, lbT.z), new Vector2(rtT.x, rtT.z));
    }

    private void DrawLine(Vector2 p1, Vector2 p2)
    {
        var leftBottom = new Vector3(p1.x, 10f, p1.y);
        var leftTop = new Vector3(p1.x, 10f, p2.y);
        var rightBottom = new Vector3(p2.x, 10f, p1.y);
        var rightTop = new Vector3(p2.x, 10f, p2.y);

        Handles.color = Color.red;
        
        Handles.DrawLine(leftBottom, rightBottom);
        Handles.DrawLine(rightBottom, rightTop);
        Handles.DrawLine(rightTop, leftTop);
        Handles.DrawLine(leftTop, leftBottom);
    }
}
