using System.Linq;
using UnityEngine;

public enum ERelative
{
    Right,
    Left,
    Top,
    Bottom
}

public class CorridorNode : BSP_Node
{
    private int _width;
    private int _interval;

    public CorridorNode(RoomNode node1, RoomNode node2, int width, int distanceFromWall) : base(null)
    {
        _width = width;
        _interval = distanceFromWall;
        
        GenerateCorridor(node1, node2);
    }

    private void GenerateCorridor(RoomNode node1, RoomNode node2)
    {
        var relative = GetRelativePosition(node1, node2);

        switch (relative)
        {
            case ERelative.Right:
                ConnectCorridorLeftRight(node2, node1);
                break;
            
            case ERelative.Left:
                ConnectCorridorLeftRight(node1, node2);
                break;
            
            case ERelative.Top:
                break;
            
            case ERelative.Bottom:
                break;
            
            default:
                return;
        }
    }

    private void ConnectCorridorLeftRight(RoomNode leftRoom, RoomNode rightRoom)
    {
        // 연결할 좌측 노드 찾기
        RoomNode selectedLeftRoom = null;
        var leftRoomList = NodeUtility.GetAllLeafNode(leftRoom)
            .OrderByDescending(x => x.RoomPosition.TR.x).ToList();

        if (leftRoomList.Count <= 0)
        {
            selectedLeftRoom = leftRoom;
        }
        else
        {
            var maxX = leftRoomList[0].RoomPosition.TR.x;

            leftRoomList = leftRoomList.Where(
                    x => Mathf.Abs(maxX - x.RoomPosition.TR.x) < 10)
                    .ToList();

            var index = Random.Range(0, leftRoomList.Count);
            selectedLeftRoom = leftRoomList[index];
            
        }
        
        // 연결할 우측 노드 찾기
        RoomNode selectedRightRoom = null;
        var rightRoomList = NodeUtility.GetAllLeafNode(rightRoom).Where(x =>
                FindConnectPositionInLeftRight(selectedLeftRoom.RoomPosition, x.RoomPosition) != -1)
            .OrderBy(x => x.RoomPosition.BR.x).ToList();

        var pos = -1;
        if (rightRoomList.Count <= 0)
        {
            // 현재 좌측 노드와 연결할 우측 노드가 존재하지 않는 상태. 파라미터로 넘어온 
            // 우측 노드를 기준으로 연결 가능한 좌측 노드를 다시 찾는다.
            selectedRightRoom = rightRoom;
            pos = FindConnectPositionInLeftRight(selectedLeftRoom.RoomPosition,
                selectedRightRoom.RoomPosition);

            while (leftRoomList.Count > 0 && pos == -1)
            {
                leftRoomList.Remove(selectedLeftRoom);
                selectedLeftRoom = leftRoomList[0];

                pos = FindConnectPositionInLeftRight(selectedLeftRoom.RoomPosition,
                    selectedRightRoom.RoomPosition);
            }
        }
        else
        {
            // 연결 가능한 노드가 존재하니까, Range(0, rightRoomList.count)를 해서 랜덤하게 선택하던, 혹은 [0]으로 고정 선택하도록 변경
            selectedRightRoom = rightRoomList[0];
            pos = FindConnectPositionInLeftRight(selectedLeftRoom.RoomPosition,
                selectedRightRoom.RoomPosition);
        }

        Pos = (pos == -1)
            ? null
            : new NodePosition(
                new Vector2Int(selectedLeftRoom.RoomPosition.BR.x, pos),
                new Vector2Int(selectedRightRoom.RoomPosition.BL.x, pos + _width));
    }

    /// <summary>
    /// 복도를 생성할 중앙 지점을 찾는다.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private int FindConnectPositionInLeftRight(NodePosition left, NodePosition right)
    {
        var leftTop = left.TR;
        var leftBottom = left.BR;
        var rightTop = right.TL;
        var rightBottom = right.BL;
        
        // 좌측 방이 우측 방보다 작다면
        if (leftTop.y <= rightTop.y && leftBottom.y >= rightBottom.y)
        {
            var bt = leftBottom + new Vector2Int(0, _interval);
            var t = leftTop - new Vector2Int(0, _interval + _width);

            return CalculateMidPoint(bt, t).y;
        }
        
        // 좌측 방이 우측 방보다 크다면
        if (leftTop.y >= rightTop.y && leftBottom.y <= rightBottom.y)
        {
            var bt = rightBottom + new Vector2Int(0, _interval);
            var t = rightTop - new Vector2Int(0, _interval + _width);

            return CalculateMidPoint(bt, t).y;
        }
        
        // 좌측 방이 우측 방보다 상단에 위치한다면
        if (leftTop.y >= rightTop.y && leftBottom.y >= rightBottom.y)
        {
            var bt = leftBottom + new Vector2Int(0, _interval);
            var t = rightTop - new Vector2Int(0, _interval + _width);

            return CalculateMidPoint(bt, t).y;
        }
        
        // 좌측 방이 우측 방보다 하단에 위치한다면
        if (leftTop.y <= rightTop.y && leftBottom.y <= rightBottom.y)
        {
            var bt = rightBottom + new Vector2Int(0, _interval);
            var t = leftTop - new Vector2Int(0, _interval + _width);

            return CalculateMidPoint(bt, t).y;
        }
        
        return -1;
    }

    private Vector2Int CalculateMidPoint(Vector2Int v1, Vector2Int v2)
    {
        return (v1 + v2) / 2;
    }

    private ERelative GetRelativePosition(RoomNode node1, RoomNode node2)
    {
        var mid1 = (node1.Pos.BL + node1.Pos.TR) / 2;
        var mid2 = (node2.Pos.BL + node2.Pos.TR) / 2;

        var angle = Mathf.Atan2(mid2.y - mid1.y, mid2.x - mid1.x);
        angle *= Mathf.Rad2Deg;
        
        if (45f <= angle && angle <= 135f) return ERelative.Top;
        if (-135f <= angle && angle <= -45f) return ERelative.Bottom;
        if (-45f <= angle && angle >= 45f) return ERelative.Right;
        
        return ERelative.Left;
    }
    
}
