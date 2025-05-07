using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Create Setting")] 
    [SerializeField] private DungeonData dungeonData;
    
    private List<RoomNode> _roomNodeList;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        var bsp = new BinarySpacePartitioning();
        _roomNodeList = bsp.BSP(dungeonData);
        
        GetComponent<LineDisplay>().DisplayLine(_roomNodeList);
    }
    
}
