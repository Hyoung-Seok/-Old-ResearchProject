using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset visualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/TestWindow")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<TestWindow>();
        wnd.titleContent = new GUIContent("TestWindow");
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;
        visualTreeAsset.CloneTree(root);

        var label = root.Q<Label>("TextInfo");
        var button = root.Q<Button>("TestButton");

        button.clicked += () =>
        {
            Debug.Log(label.text);
        };
    }
}
