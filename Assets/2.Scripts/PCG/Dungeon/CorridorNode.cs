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
                ConnectCorridorBottomTop(node1, node2);
                break;
            
            case ERelative.Bottom:
                ConnectCorridorBottomTop(node2, node1);
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
            // 연결 가능한 노드가 존재하니까, 가장 가까운 노드를 선택
            selectedRightRoom = rightRoomList[0];
            pos = FindConnectPositionInLeftRight(selectedLeftRoom.RoomPosition,
                selectedRightRoom.RoomPosition);
        }

        Pos = (pos == -1)
            ? null
            : new NodePosition(
                new Vector2Int(selectedLeftRoom.RoomPosition.BR.x, pos),
                new Vector2Int(selectedRightRoom.RoomPosition.TL.x, pos + _width));
    }

    private void ConnectCorridorBottomTop(RoomNode bottom, RoomNode top)
    {
        RoomNode selectedBottomNode = null;
        var bottomList = NodeUtility.GetAllLeafNode(bottom)
            .OrderByDescending(x => x.RoomPosition.TL.y).ToList();

        if (bottomList.Count <= 1)
        {
            selectedBottomNode = bottom;
        }
        else
        {
            var maxY = bottomList[0].RoomPosition.TL.y;
            bottomList = bottomList.Where(x => Mathf.Abs(maxY - x.RoomPosition.TL.y) < 10).ToList();

            selectedBottomNode = bottomList[Random.Range(0, bottomList.Count)];
        }

        RoomNode selectedTopNode = null;
        var topList = NodeUtility.GetAllLeafNode(top)
            .Where(x =>
                FindConnectPositionInBottomTop(selectedBottomNode.RoomPosition, x.RoomPosition) !=
                -1).OrderBy(x => x.RoomPosition.BR.y).ToList();

        var pos = -1;
        if (topList.Count <= 0)
        {
            selectedTopNode = top;
            pos = FindConnectPositionInBottomTop(selectedBottomNode.RoomPosition,
                selectedTopNode.RoomPosition);

            while (pos != -1 && bottomList.Count > 0)
            {
                bottomList.Remove(selectedBottomNode);
                selectedBottomNode = bottomList[0];
                
                pos = FindConnectPositionInBottomTop(selectedBottomNode.RoomPosition,
                    selectedTopNode.RoomPosition);
            }
        }
        else
        {
            selectedTopNode = topList[0];
            pos = FindConnectPositionInBottomTop(selectedBottomNode.RoomPosition,
                selectedTopNode.RoomPosition);
        }

        Pos = (pos == -1)
            ? null
            : new NodePosition(
                new Vector2Int(pos, selectedBottomNode.RoomPosition.TL.y),
                new Vector2Int(pos + _width, selectedTopNode.RoomPosition.BR.y)
            );
    }
    
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
        if (leftBottom.y >= rightBottom.y && leftBottom.y <= rightTop.y)
        {
            var bt = leftBottom + new Vector2Int(0, _interval);
            var t = rightTop - new Vector2Int(0, _interval + _width);

            return CalculateMidPoint(bt, t).y;
        }
        
        // 좌측 방이 우측 방보다 하단에 위치한다면
        if (leftTop.y >= rightBottom.y && leftTop.y <= rightTop.y)
        {
            var bt = rightBottom + new Vector2Int(0, _interval);
            var t = leftTop - new Vector2Int(0, _interval + _width);

            return CalculateMidPoint(bt, t).y;
        }
        
        return -1;
    }

    private int FindConnectPositionInBottomTop(NodePosition bottom, NodePosition top)
    {
        var bottomLeft = bottom.TL.x;
        var bottomRight = bottom.TR.x;
        var topLeft = top.BL.x;
        var topRight = top.BR.x;
        
        // 상단에 위치한 방이 더 작다면
        if (topLeft <= bottomLeft && topRight >= bottomRight)
        {
            var min = new Vector2Int(topLeft + _interval, 0);
            var max = new Vector2Int(topRight - (_interval + _width), 0);

            return CalculateMidPoint(min, max).x;
        }
        
        // 상단에 위치한 방이 더 작다면
        if (topLeft >= bottomLeft && topRight <= bottomRight)
        {
            var min = new Vector2Int(bottomLeft + _interval, 0);
            var max = new Vector2Int(bottomRight - (_interval + _width), 0);

            return CalculateMidPoint(min, max).x;
        }
        
        // 상단 방이 우측에 위치할 때
        if (bottomLeft <= topLeft && topLeft <= bottomRight)
        {
            var min = new Vector2Int(topLeft + _interval, 0);
            var max = new Vector2Int(bottomRight - (_interval + _width), 0);

            return CalculateMidPoint(min, max).x;
        }
        
        // 상단 방이 좌측에 위치할 때
        if(bottomLeft <= topRight && topRight <= bottomRight)
        {
            var min = new Vector2Int(bottomLeft + _interval, 0);
            var max = new Vector2Int(topRight - (_interval + _width), 0);

            return CalculateMidPoint(min, max).x;
        }
        
        return -1;
    }

    private Vector2Int CalculateMidPoint(Vector2Int v1, Vector2Int v2)
    {
        return (v1 + v2) / 2;
    }

    private ERelative GetRelativePosition(RoomNode node1, RoomNode node2)
    {
        var mid1 = (node1.Pos.TR + node1.Pos.BL) / 2;
        var mid2 = (node2.Pos.TR + node2.Pos.BL) / 2;

        var angle = Mathf.Atan2(mid2.y - mid1.y, mid2.x - mid1.x);
        angle *= Mathf.Rad2Deg;
        
        if (45f < angle && angle < 135f) return ERelative.Top;
        if (-135f < angle && angle < -45f) return ERelative.Bottom;
        if (-45f < angle && angle > 45f) return ERelative.Right;
        return ERelative.Left;
    }
    
}
