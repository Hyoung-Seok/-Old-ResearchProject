using UnityEngine;

public class FalloffMap : MonoBehaviour
{
    [Header("Falloff Map")] 
    [SerializeField, Range(1f, 5f)] private float dampingCurveWeight;       // 곡선의 가파름 제어
    [SerializeField, Range(0f, 1f)] private float attenuationRatio;         // 곡선의 감쇠 비율을 설정
    [SerializeField] private Vector2 centerOffset;
    [SerializeField, Range(0f, 5f)] private float radius;

    public float[,] GenerateFalloffMap((int width, int height) size)
    {
        var fallOffMap = new float[size.height, size.width];
        var center = new Vector2
            ((float)size.width / 2 + centerOffset.x, (float)size.height / 2 + centerOffset.y);

        for (var y = 0; y < size.height; ++y)
        {
            for (var x = 0; x < size.width; ++x)
            {
                var distance = Vector2.Distance(new Vector2(x, y), center);
                var gradient = distance / (size.width * radius);
                gradient = Mathf.Clamp01(gradient);

                fallOffMap[y, x] = Evaluate(gradient);
            }
        }

        return fallOffMap;
    }

    private float Evaluate(float val)
    {
        var numerator = Mathf.Pow(val, dampingCurveWeight);
        var denominator = 
            numerator + Mathf.Pow(attenuationRatio - attenuationRatio * val, dampingCurveWeight);

        return numerator / denominator;
    }
}
