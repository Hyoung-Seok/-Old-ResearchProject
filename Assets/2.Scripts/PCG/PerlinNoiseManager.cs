using System;
using UnityEngine;

public class PerlinNoiseManager : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private FalloffMap fallOffGenerator;
    [SerializeField] private SpriteRenderer noiseRenderer;
    [SerializeField] private SpriteRenderer fallOffRenderer;
    
    [Header("Perlin Noise")] 
    [SerializeField] private NoiseData noiseData;
    [SerializeField] private ColorData colorData;

    private float[,] _noiseMap;
    private float[,] _fallOffMap;

    public void GeneratePerlinNoiseMap()
    {
        _noiseMap = NoiseMapGenerator.PerlinNoise(noiseData);

        var noiseTex = NoiseTextureGenerator.GenerateNoiseTexture(_noiseMap, colorData.NoiseColor);
        noiseRenderer.sprite = Sprite.Create(noiseTex,
            new Rect(0, 0, noiseData.Width, noiseData.Height), new Vector2(0.5f, 0.5f));
    }

    public void GenerateFallOffMap()
    {
        _fallOffMap = fallOffGenerator.GenerateFalloffMap((noiseData.Width, noiseData.Height));

        var fallOffTex = NoiseTextureGenerator.GenerateFalloffMapTexture(_fallOffMap);
        fallOffRenderer.sprite = Sprite.Create(fallOffTex,
            new Rect(0, 0, noiseData.Width, noiseData.Height), new Vector2(0.5f, 0.5f));
    }

    private void OnValidate()
    {
        GeneratePerlinNoiseMap();
    }
}
