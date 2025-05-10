using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "PCG/DungeonData")]
public class DungeonData : ScriptableObject
{
    [Header("BSP Setting")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int roomMinWidth;
    [SerializeField] private int roomMinHeight;
    [SerializeField] private int iteration;
    
    [Header("Split Setting")]
    [SerializeField] private float horizontalRatio = 1.25f;
    [SerializeField] private float verticalRatio = 0.8f;
    [SerializeField, Range(0f, 1f)] private float splitRange = 0.5f; 

    [Header("Room Generate Setting")] 
    [SerializeField] private Vector2Int offset;
    [SerializeField, Range(0f, 0.4f)] private float bottomLeftWeight;
    [SerializeField, Range(0.6f, 0.9f)] private float topRightWeight;

    public int Width => width;
    public int Height => height;
    public int RoomMinWidth => roomMinWidth;
    public int RoomMinHeight => roomMinHeight;
    public int Iteration => iteration;
    public Vector2Int Offset => offset;
    public float BottomLeftWeight => bottomLeftWeight;
    public float TopRightWeight => topRightWeight;
    public float HorizontalRatio => horizontalRatio;
    public float VerticalRatio => verticalRatio;
    public float SplitRange => splitRange;
}
