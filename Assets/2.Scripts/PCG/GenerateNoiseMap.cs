using System;
using UnityEngine;

public class GenerateNoiseMap : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private SpriteRenderer noiseSprite;
    
    [Header("Setting")] 
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float noiseScale = 0.1f;

    private float[,] _noiseMap;
    private CustomPerlinNoise _customPerlin;

    private void Start()
    {
        _customPerlin = new CustomPerlinNoise();
        
        SetPerlinNoise2DArray();
        noiseSprite.sprite = Sprite.Create(CreateNoiseTexture(), new Rect(0, 0, width, height),
            new Vector2(0.5f, 0.5f));
    }

    private void SetPerlinNoise2DArray()
    {
        _noiseMap = new float[height, width];
        
        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                var sampleX = (float)x / width * noiseScale;
                var sampleY = (float)y / height * noiseScale;

                var raw = _customPerlin.PerlinNoise(sampleX, sampleY);
                var normalized = Mathf.Clamp01((raw + 1f) * 0.5f);
                _noiseMap[y, x] = normalized;
                
                Debug.Log(_noiseMap[y, x]);
            }
        }
    }

    private Texture2D CreateNoiseTexture()
    {
        var tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Bilinear;
        
        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                var color = new Color(_noiseMap[y, x], _noiseMap[y, x], _noiseMap[y, x]);
                tex.SetPixel(x, y, color);
            }
        }
        
        tex.Apply();
        return tex;
    }
}
