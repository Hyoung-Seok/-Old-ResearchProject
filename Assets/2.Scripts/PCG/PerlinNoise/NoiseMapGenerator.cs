using System;
using UnityEngine;
using Random = System.Random;

public static class NoiseMapGenerator
{
    private static int _seed;
    private static float _scale;
    private static int _width;
    private static int _height;
    private static int _octave;
    private static float _lacunarity;
    private static float _persistence;
    
    public static float[,] PerlinNoise(NoiseData data)
    {
        InitNoiseData(data);
        
        if (_scale <= 0)
            _scale = 0.0001f;

        var noiseMap = new float[_height, _width];
        
        // 옥타브 오프셋 벡터 생성
        // 입력받은 x,y 위치에서 각 옥타브 마다 랜덤한 오프셋 위치를 주기 위함.
        var octaveOffset = new Vector2[_octave];
        var prng = new Random(_seed);        // 같은 seed에 대해 같은 난수를 생성하는 의사 난수 생성기

        for (var i = 0; i < _octave; ++i)
        {
            float xPos = prng.Next(-100000, 100000);
            float yPos = prng.Next(-100000, 100000);

            octaveOffset[i] = new Vector2(xPos, yPos);
        }

        var halfWidth = _width / 2f;
        var halfHeight = _height / 2f;
        var minHeight = float.MaxValue;
        var maxHeight = float.MinValue;
        
        // PerlinNoise 계산 시작
        for (var y = 0; y < _height; ++y)
        {
            for (var x = 0; x < _width; ++x)
            {
                // 주파수와 진폭
                var frequency = 1f;
                var amplitude = 1f;
                var noiseHeight = 0f;
                
                // 옥타브 수만큼 반복
                for (var i = 0; i < _octave; ++i)
                {
                    // perlinNoise의 좌표를 중앙 기준으로 정규화.
                    var sampleX = (x - halfWidth) / _scale * frequency + octaveOffset[i].x;
                    var sampleY = (y - halfHeight) / _scale * frequency + octaveOffset[i].y;

                    var perlin = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeight += perlin * amplitude;

                    frequency *= _lacunarity;
                    amplitude *= _persistence;
                }

                noiseMap[y, x] = noiseHeight;
                
                // 높이 정규화
                if (noiseHeight < minHeight) minHeight = noiseHeight;
                if (noiseHeight > maxHeight) maxHeight = noiseHeight;
            }
        }
        
        // 정규화 시작
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                // a, b사이에 value가 어느 정도에 위치해있는지 0.0~1.0사이의 값으로 반환
                noiseMap[y, x] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[y, x]);
            }
        }

        return noiseMap;
    }
    
    private static void InitNoiseData(NoiseData data)
    {
        _seed = data.Seed;
        _scale = data.Scale;
        _width = data.Width;
        _height = data.Height;
        _octave = data.Octave;
        _lacunarity = data.Lacunarity;
        _persistence = data.Persistance;
    }
}
