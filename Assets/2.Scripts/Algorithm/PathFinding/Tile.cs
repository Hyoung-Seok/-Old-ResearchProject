using UnityEngine;

public class Tile
{
    public int Weight { get; private set; }
    public Material Mat { get; private set; }

    private readonly Color _originColor;

    public Tile(GameObject obj, int weight)
    {
        Mat = obj.GetComponent<Renderer>().material;
        _originColor = Mat.color;
        
        Weight = weight;
    }

    public void ResetColor()
    {
        Mat.color = _originColor;
    }
}
