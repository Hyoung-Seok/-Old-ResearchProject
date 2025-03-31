using UnityEngine;

public class Tile
{
    public int Weight { get; private set; }
    public Material Mat { get; private set; }

    public Tile(GameObject obj, int weight)
    {
        Mat = obj.GetComponent<Renderer>().material;
        Weight = weight;
    }
}
