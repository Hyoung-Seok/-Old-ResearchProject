using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private Vector2Int _offset;
    private float _minWeight;
    private float _maxWeight;

    public void GenerateRoom(List<RoomNode> leafNode, DungeonData data)
    {
        _offset = data.Offset;
        _minWeight = data.BottomLeftWeight;
        _maxWeight = data.TopRightWeight;

        foreach (var node in leafNode)
        {
            CreateRoomSpace(node);
        }
    }

    private void CreateRoomSpace(RoomNode node)
    {
        var curPos = node.Pos;

        // 최소, 최대 범위 계산
        var min = new Vector2Int(curPos.BL.x + _offset.x, curPos.BL.y + _offset.y);
        var max = new Vector2Int(curPos.TR.x - _offset.x, curPos.TR.y - _offset.y);
        
        // 방이 생성될 수 있는 길이 구하기
        var roomWidth = max.x - min.x;
        var roomHeight = max.y - min.y;

        var roomBL = new Vector2Int(
            Random.Range(min.x, min.x + (int)(roomWidth * _minWeight)),
            Random.Range(min.y, min.y + (int)(roomHeight * _minWeight)));
        
        // BL보다 무조건 커야함. 
        var minTRX = roomBL.x + (int)(roomWidth * _maxWeight);
        var minTRY = roomBL.y + (int)(roomHeight * _maxWeight);
        
        var roomTR = new Vector2Int(
            Random.Range(minTRX, max.x),
            Random.Range(minTRY, max.y));

        node.AddRoomPosition(new NodePosition(roomBL, roomTR));
    }
}
