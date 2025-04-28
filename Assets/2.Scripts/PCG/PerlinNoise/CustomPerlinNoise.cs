using UnityEngine;

public class CustomPerlinNoise
{
    public float PerlinNoise(float x, float y)
    {
        //  p1(x1, y1) ------ p2(x2, y1)
        //  |                       |
        //  |                       |
        //  p3(x1, y2) ------ p4(x2, y2)
        var xToInt = Mathf.FloorToInt(x);
        var yToInt = Mathf.FloorToInt(y);

        // 입력값에서 인접한 4개의 정수 그리드 좌표
        var vertex = new Vector2Int[]
        {
            new(xToInt, yToInt),
            new(xToInt + 1, yToInt),
            new(xToInt, yToInt + 1),
            new(xToInt + 1, yToInt + 1)
        };
        var gradient = new Vector2[4];      // 각 격자의 꼭지점 그라디언트 벡터를 저장할 변수
        var dis = new Vector2[4];           // 꼭지점에서 입력값으로 향하는 벡터를 저장할 변수
        var influence = new float[4];       // 영향력을 저장할 변수

        for (var i = 0; i < 4; ++i)
        {
            gradient[i] = GetGradient(vertex[i]);
            dis[i] = new Vector2(x, y) - vertex[i];
            influence[i] = Vector2.Dot(gradient[i], dis[i]);
        }
        
        // 보간 시작
        var fx = Fade(x - xToInt);
        var fy = Fade(y - yToInt);

        // x축 보간
        var i1 = Mathf.Lerp(influence[0], influence[1], fx);    // y0 줄
        var i2 = Mathf.Lerp(influence[2], influence[3], fx);    // y1 줄
        
        // y축 보간 후 결과 반환
        return Mathf.Lerp(i1, i2, fy);
    }

    // 정수 좌표마다, 랜덤한 방향 벡터 반환
    private Vector2 GetGradient(Vector2Int vec)
    {
        var hash = Hash(vec.x, vec.y);
        var angle = (hash % 360) * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
    
    // 정수 입력마다 고정된 값을 생성하면서, 전체적으로 보면 무작위성처럼 보이는 값을 반환
    // (Random.Range는 완전 랜덤한 값을 반환하기 때문에, 의사 난수(Deterministic Hash) 기반이 아님)
    private int Hash(int x, int y)
    {
        var seed = 63689;
        var hash = x;

        hash = hash * seed + y;
        hash = (hash << 13) ^ hash;
        return (hash * (hash * hash * 15731 + 789221) + 1376312589) & 0x7fffffff;
    }
    
    // Ken Perlin의 Fade 함수 수식
    private float Fade(float t)
    {
        return 6 * Mathf.Pow(t, 5) - 15 * Mathf.Pow(t, 4) + 10 * Mathf.Pow(t, 3);
    }
}
