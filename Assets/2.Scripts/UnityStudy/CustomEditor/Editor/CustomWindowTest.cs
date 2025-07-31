using System;
using UnityEditor;
using UnityEngine;

public class CustomWindowTest : EditorWindow
{
    [MenuItem("Tools/Window Test", validate = false, priority = 100)]
    private static void Init()
    {
        var window = GetWindow<CustomWindowTest>();

        // 상단 탭의 제목 설정
        window.titleContent = new GUIContent("Test");
        
        // 윈도우 창의 최소, 최대 크기 설정
        window.minSize = new Vector2(200, 200);
        window.maxSize = new Vector2(1024, 1024);
        
        // 창의 위치와 크기 설정(x, y, width, height)
        window.position = new Rect(400, 400, 350, 600);
        
        // 현재 커스텀 윈도우를 선택 상태로 만듦
        window.Focus();
        
        // 값이 true면 마우스가 창 위에서 움직이기만 해도 OnGUI() 호출
        // -> 마우스가 창 위에서 움직일 때 Event.mousePosition을 갱신
        window.wantsMouseMove = false;
        
        window.Show();
        
        // 윈도우 창 닫기
        //window.Close();
    }

    private string _textField = " ";
    private string _textArea = " ";

    private bool _check;

    private float _sliderValue = 0;
    
    // 드롭다운 변수
    private int _selectedIndex = 0;
    private string[] _options = { "옵션 1", "옵션 2", "옵션 3" };
    
    public void OnGUI()
    {
        // 단순한 라벨(읽기 전용 텍스트) 출력
        GUILayout.Label("My First Custom Window");
        
        // 공백
        GUILayout.Space(10);
        
        // 라벨을 가운데 정렬
        var label = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            normal = {textColor = Color.red}
        };
        
        GUILayout.Label("My First Custom Window", label);
        
        // 입력 필드 (string)
        _textField = GUILayout.TextField(_textField);
        _textArea = GUILayout.TextArea(_textArea);  // 여러 줄
        
        // 입력 필드 (bool)
        _check = GUILayout.Toggle(_check, "Check");
        
        // 버튼 (클릭 시 true 반환)
        GUILayout.Button("Button");
        
        // 슬라이더(가로) => (float, min, max)
        _sliderValue = GUILayout.HorizontalSlider(_sliderValue, 0, 100);
        
        // 문자열 기반 드롭다운
        _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _options);
    }
}
