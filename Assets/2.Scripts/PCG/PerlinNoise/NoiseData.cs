using System;
using UnityEngine;

[Serializable]
public class NoiseData
{
    [Header("Setting")] 
    public int Seed;
    public float Scale;
    
    [Header("Noise Map Size")]
    public int Width;
    public int Height;

    [Header("Perlin Setting")] 
    public int Octave;
    [Range(1f, 5f)] public float Lacunarity;
    [Range(0.1f, 0.9f)] public float Persistance;
}
