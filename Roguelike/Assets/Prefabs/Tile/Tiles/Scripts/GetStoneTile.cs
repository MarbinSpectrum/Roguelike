using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStoneTile : GetTile
{
    public TileBundle woodFloor;
    public TileBundle matFloor;
    public TileBundle stoneStairTile;

    public override TileData GetTileData(TileType[,] pTile)
    {
        if (pTile[1, 1] == TileType.Stone_Floor || pTile[1, 1] == TileType.Stone_Wall)
        {
            return baseTile.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Wood_Floor || pTile[1, 1] == TileType.Wood_Wall)
        {
            return woodFloor.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Mat_Floor)
        {
            return matFloor.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Stone_Stair)
        {
            return stoneStairTile.GetTileData(pTile);
        }
        return nullTile;
    }
}
