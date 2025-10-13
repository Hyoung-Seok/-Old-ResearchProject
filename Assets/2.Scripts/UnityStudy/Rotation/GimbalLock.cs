using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GimbalLock : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private RotationInfo[] rotationInfos;
    [SerializeField] private Transform objectTF;
    [SerializeField] private Transform axis;
    [SerializeField] private Transform[] axisObject;

    public void ValueChange(int index)
    {
        if (index >= rotationInfos.Length)
            return;

        var value = rotationInfos[index].Slider.value;
        rotationInfos[index].Value.text = value.ToString();

        var angle = objectTF.eulerAngles;
        
        switch (index)
        {
            case 0:
                objectTF.eulerAngles = new Vector3(value, angle.y, angle.z);
                break;
            
            case 1:
                objectTF.eulerAngles = new Vector3(angle.x, value, angle.z);
                break;
            
            case 2:
                objectTF.eulerAngles = new Vector3(angle.x, angle.y, value);
                break;
            
            default:
                return;
        }

        axis.eulerAngles = objectTF.eulerAngles;
        
        axisObject[0].localRotation = Quaternion.Euler(value, 0f, 0f); // X축
        axisObject[1].localRotation = Quaternion.Euler(0f, value, 0f); // Y축
        axisObject[2].localRotation = Quaternion.Euler(0f, 0f, value); // Z축
    }
}

[Serializable]
struct RotationInfo
{
    public Slider Slider;
    public TextMeshProUGUI Value;
}
