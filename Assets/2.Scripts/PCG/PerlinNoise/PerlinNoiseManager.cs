using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PerlinNoiseManager : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private FalloffMap fallOffGenerator;
    [SerializeField] private SpriteRenderer noiseRenderer;
    [SerializeField] private SpriteRenderer fallOffRenderer;
    [SerializeField] private SpriteRenderer fractalRenderer;
    
    [Header("Perlin Noise")] 
    [SerializeField] private NoiseData noiseData;
    [SerializeField] private ColorData colorData;

    [Header("Terrain Setting")] 
    [SerializeField] private Vector2[] blendingRanges;
    [SerializeField] private Terrain terrain;
    [SerializeField] private Material terrainShader;
    [SerializeField] private float maxHeight = 50f;
    
    [Header("Shader Properties")] 
    [SerializeField] private List<string> blendingRangeNames;
    private static readonly int MinMaxHeight = Shader.PropertyToID("_MinMaxHeight");
    
    private float[,] _noiseMap;
    private float[,] _fallOffMap;
    private float[,] _fractalMap;

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

    public void ApplyFallOffMap()
    {
        _fractalMap = new float[noiseData.Height, noiseData.Width];
        
        for (var y = 0; y < noiseData.Height; ++y)
        {
            for (var x = 0; x < noiseData.Width; ++x)
            {
                _fractalMap[y, x] = Mathf.Clamp01(_noiseMap[y, x] - _fallOffMap[y, x]);
            }
        }

        var tex = NoiseTextureGenerator.GenerateNoiseTexture(_fractalMap, colorData.NoiseColor);
        fractalRenderer.sprite = Sprite.Create(tex, 
            new Rect(0, 0, noiseData.Width, noiseData.Height), new Vector2(0.5f, 0.5f));
    }

    public void CreateTerrain()
    {
        var terrainData = terrain.terrainData;

        terrainData.heightmapResolution = Mathf.Max(noiseData.Width, noiseData.Height);
        terrainData.size = new Vector3(noiseData.Width, maxHeight, noiseData.Height);
        terrainData.SetHeights(0, 0, _fractalMap);
        
        SetShaderProperty();
    }

    private void SetShaderProperty()
    {
        terrainShader.SetVector(MinMaxHeight, new Vector2(0f, maxHeight));

        if (blendingRangeNames.Count != blendingRanges.Length) return;

        for (var i = 0; i < blendingRangeNames.Count; ++i)
        {
            var id = Shader.PropertyToID(blendingRangeNames[i]);
            terrainShader.SetVector(id, blendingRanges[i]);
        }
    }
}
