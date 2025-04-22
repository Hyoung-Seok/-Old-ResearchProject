using UnityEngine;

public class PerlinNoiseManager : MonoBehaviour
{
    [Header("Perlin Noise")] 
    [SerializeField] private NoiseData noiseData;
    [SerializeField] private ColorData colorData;
    [SerializeField] private SpriteRenderer noiseRenderer;

    private float[,] _noiseMap;

    public void GeneratePerlinNoiseMap()
    {
        _noiseMap = NoiseMapGenerator.PerlinNoise(noiseData);

        var noiseTex = NoiseTextureGenerator.GenerateNoiseTexture(_noiseMap, colorData.NoiseColor);
        noiseRenderer.sprite = Sprite.Create(noiseTex,
            new Rect(0, 0, noiseData.Width, noiseData.Height), new Vector2(0.5f, 0.5f));
    }
}
