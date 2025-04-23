using UnityEngine;

public static class NoiseTextureGenerator
{
    private static ColorHeight[] _colorHeights;
    
    public static Texture2D GenerateNoiseTexture(float[,] noiseMap, ColorHeight[] colorHeights)
    {
        var tex = new Texture2D(noiseMap.GetLength(1), noiseMap.GetLength(0));
        _colorHeights = colorHeights;

        for (var y = 0; y < noiseMap.GetLength(0); ++y)
        {
            for (var x = 0; x < noiseMap.GetLength(1); ++x)
            {
                var color = ApplyHeightColor(noiseMap[y, x]);
                tex.SetPixel(x, y, color);
            }
        }
        
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Apply();
        
        return tex;
    }

    public static Texture2D GenerateFalloffMapTexture(float[,] falloff)
    {
        var tex = new Texture2D(falloff.GetLength(1), falloff.GetLength(0));

        for (var y = 0; y < falloff.GetLength(0); ++y)
        {
            for (var x = 0; x < falloff.GetLength(1); ++x)
            {
                var val = falloff[y, x];
                var color = new Color(val, val, val);
                tex.SetPixel(x, y, color);
            }
        }
        
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Apply();

        return tex;
    }
    
    private static Color ApplyHeightColor(float height)
    {
        foreach (var colorHeight in _colorHeights)
        {
            if (colorHeight.Height > height)
            {
                return colorHeight.Color;
            }
        }
        
        return Color.black;
    }
}
