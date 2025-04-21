using UnityEngine;

public class PerlinNoiseManager : MonoBehaviour
{
    [Header("Perlin Noise")] 
    [SerializeField] private NoiseData noiseData;
    [SerializeField] private SpriteRenderer noiseRenderer;

    private float[,] _noiseMap;

    public void GeneratePerlinNoiseMap()
    {
        _noiseMap = NoiseMapGenerator.PerlinNoise(noiseData);
        noiseRenderer.sprite = Sprite.Create(NoiseTextureGenerator.GenerateNoiseTexture(_noiseMap),
            new Rect(0, 0, noiseData.Width, noiseData.Height), new Vector2(0.5f, 0.5f));
    }
}
