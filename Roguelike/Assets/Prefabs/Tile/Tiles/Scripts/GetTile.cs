using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTile : MonoBehaviour
{
    public TileBundle stoneBunble;
    public TileBundle stoneStairTile;
    public TileBundle woodBunble;
    public TileBundle matBunble;
    public TileBundle concreteBunble;

    public TileData nullTile;

    public TileData GetTileData(TileType[,] pTile)
    {
        if (pTile[1, 1] == TileType.Stone_Floor || 
            pTile[1, 1] == TileType.Stone_Wall || 
            pTile[1, 1] == TileType.Stone_Pillar)
        {
            return stoneBunble.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Stone_Stair)
        {
            return stoneStairTile.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Wood_Floor || pTile[1, 1] == TileType.Wood_Wall)
        {
            return woodBunble.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Concrete_Floor || pTile[1, 1] == TileType.Concrete_Wall)
        {
            return concreteBunble.GetTileData(pTile);
        }
        else if (pTile[1, 1] == TileType.Mat_Floor)
        {
            return matBunble.GetTileData(pTile);
        }
        return nullTile;
    }
}
