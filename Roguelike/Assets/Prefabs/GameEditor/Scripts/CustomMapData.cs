using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 에디터로 만든 맵 정보
////////////////////////////////////////////////////////////////////////////////
[System.Serializable]
public class CustomMapData
{
    public uint roomW;
    public uint roomH;

    public List<TileType> tiles;
    public List<Obj> objs;

    public CustomMapData(uint pW, uint pH, TileGroup pTileGroup, ObjGroup pObjGroup)
    {
        roomW = pW;
        roomH = pH;
        tiles = pTileGroup.GetTiles();
        objs = pObjGroup.GetObjs();
    }
}
