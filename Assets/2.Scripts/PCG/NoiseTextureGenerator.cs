using UnityEngine;

public static class NoiseTextureGenerator
{
    public static Texture2D GenerateNoiseTexture(float[,] noiseMap)
    {
        var tex = new Texture2D(noiseMap.GetLength(1), noiseMap.GetLength(0));
        tex.filterMode = FilterMode.Bilinear;

        for (var y = 0; y < noiseMap.GetLength(0); ++y)
        {
            for (var x = 0; x < noiseMap.GetLength(1); ++x)
            {
                var value = noiseMap[y, x];
                var color = new Color(value, value, value);
                tex.SetPixel(x, y, color);
            }
        }
        
        tex.Apply();
        return tex;
    }
}
