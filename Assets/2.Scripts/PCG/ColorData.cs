using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "PCG/ColorData")]
public class ColorData : ScriptableObject
{
    public ColorHeight[] NoiseColor;
}

[Serializable]
public struct ColorHeight
{
    [Range(0f, 1f), SerializeField] private float height;
    [SerializeField] private Color color;

    public float Height => height;
    public Color Color => color;
}