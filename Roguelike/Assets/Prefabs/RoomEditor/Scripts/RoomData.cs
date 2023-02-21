using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �����ͷ� ���� �� ����
////////////////////////////////////////////////////////////////////////////////
[System.Serializable]
public class RoomData 
{
    public const uint roomSize = 7;

    public RoomType1 roomType1;
    public RoomType2 roomType2;
    public List<TileType> tiles;
    public List<Obj> objs;

    public RoomData(RoomType1 pRoomType1, RoomType2 pRoomType2,
        TileGroup pTileGroup, ObjGroup pObjGroup)
    {
        roomType1 = pRoomType1;
        roomType2 = pRoomType2;
        tiles = pTileGroup.GetTiles();
        objs = pObjGroup.GetObjs();
    }
}
