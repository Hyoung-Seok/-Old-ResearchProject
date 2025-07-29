using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SizeController))]
public class SizeControlEditor : Editor
{
    private SerializedProperty _scaleProp;
    private SerializedProperty _objProp;
    
    private void OnEnable()
    {
        _scaleProp = serializedObject.FindProperty("scale");
        _objProp = serializedObject.FindProperty("obj");
    }

    public override void OnInspectorGUI()
    {
        var sizeController = (SizeController)target;

        DrawDefaultInspector();

        // 직접 호출
        if (GUILayout.Button("Apply(target)"))
        {
            sizeController.ChangeScale();
        }
        
        // SerializedProperty를 통한 접근
        if (GUILayout.Button("Apply(SerializedProperty)"))
        {
            // 1. 현재 에디터 대상 오브젝트(target)의 최신 값을 직렬화 시스템에 반영.
            serializedObject.Update();
            
            // 2. SerializedProperty 값 설정
            _scaleProp.vector3Value = new Vector3(3, 3, 3);
            
            // 3. 변경 내용 적용 (쓰기 완료)
            serializedObject.ApplyModifiedProperties();
        }
    }
}
