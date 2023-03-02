using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStairBundle : TileBundle
{
    public TileData stairTile;

    public override TileData GetTileData(TileType[,] pTile)
    {
        return stairTile;
    }
}
