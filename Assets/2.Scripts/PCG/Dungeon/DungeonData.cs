using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "PCG/DungeonData")]
public class DungeonData : ScriptableObject
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int roomMinWidth;
    [SerializeField] private int roomMinHeight;
    [SerializeField] private int iteration;

    public int Width => width;
    public int Height => height;
    public int RoomMinWidth => roomMinWidth;
    public int RoomMinHeight => roomMinHeight;
    public int Iteration => iteration;
}
