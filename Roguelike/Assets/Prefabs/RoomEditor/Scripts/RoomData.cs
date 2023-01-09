using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �����ͷ� ���� �� ����
////////////////////////////////////////////////////////////////////////////////
[System.Serializable]
public class RoomData 
{
    public const uint roomSize = 11;

    public bool isStartRoom;
    public RoomType roomType;
    public List<TileType> tiles;
    public List<Obj> objs;

    public RoomData(RoomType pRoomType,bool pIsStartRoom,
        TileGroup pTileGroup, ObjGroup pObjGroup)
    {
        roomType = pRoomType;
        isStartRoom = pIsStartRoom;
        tiles = pTileGroup.GetTiles();
        objs = pObjGroup.GetObjs();
    }
}
