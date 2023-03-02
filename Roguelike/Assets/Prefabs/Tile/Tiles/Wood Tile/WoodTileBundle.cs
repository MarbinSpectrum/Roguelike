using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTileBundle : TileBundle
{
    public List<TileData> tileWall;
    public TileData woodTile;
    public TileData nullTile;

    public override TileData GetTileData(TileType[,] pTile)
    {
        if (pTile[1, 1] == TileType.Wood_Floor)
        {
            return woodTile;
        }
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
              TileManager.IsFloor(pTile[0, 1]) == false &&
              TileManager.IsFloor(pTile[2, 1]) == false &&
              TileManager.IsFloor(pTile[1, 0]) == false &&
             TileManager.IsFloor(pTile[0, 0]) &&
              TileManager.IsFloor(pTile[2, 0]))
            return tileWall[13];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[0, 0]) &&
            TileManager.IsFloor(pTile[2, 0]))
            return tileWall[13];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[0, 0]) &&
            TileManager.IsFloor(pTile[2, 0]))
            return tileWall[13];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[2, 0]))
            return tileWall[0];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
             TileManager.IsFloor(pTile[2, 1]) &&
             TileManager.IsFloor(pTile[1, 0]))
            return tileWall[1];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
             TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
             TileManager.IsFloor(pTile[1, 0]))
            return tileWall[1];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]))
            return tileWall[1];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
           TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]))
            return tileWall[1];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]))
            return tileWall[1];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
           TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[1, 1]) == false &&
           TileManager.IsFloor(pTile[2, 1]) == false &&
             TileManager.IsFloor(pTile[0, 0]))
            return tileWall[2];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[2, 1]))
            return tileWall[3];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[0, 1]))
            return tileWall[4];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
           TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[2, 2]))
            return tileWall[5];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
             TileManager.IsFloor(pTile[0, 1]) == false &&
             TileManager.IsFloor(pTile[2, 1]) == false &&
             TileManager.IsFloor(pTile[1, 0]) == false &&
             TileManager.IsFloor(pTile[2, 0]))
            return tileWall[9];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
             TileManager.IsFloor(pTile[0, 1]) == false &&
             TileManager.IsFloor(pTile[2, 1]) == false &&
             TileManager.IsFloor(pTile[1, 0]) == false &&
             TileManager.IsFloor(pTile[0, 0]))
            return tileWall[8];
        else if (TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 2]))
            return tileWall[6];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
           TileManager.IsFloor(pTile[1, 0]) == false &&
            TileManager.IsFloor(pTile[0, 2]))
            return tileWall[7];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]) == false)
            return tileWall[8];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
           TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]) == false)
            return tileWall[9];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) == false &&
            TileManager.IsFloor(pTile[1, 0]))
            return tileWall[10];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) == false &&
            TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]))
            return tileWall[11];
        else if (TileManager.IsFloor(pTile[1, 2]) &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]) == false)
            return tileWall[12];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]) == false)
            return tileWall[13];
        else if (TileManager.IsFloor(pTile[1, 2]) == false &&
            TileManager.IsFloor(pTile[0, 1]) &&
            TileManager.IsFloor(pTile[2, 1]) &&
            TileManager.IsFloor(pTile[1, 0]))
            return tileWall[14];

        return nullTile;
    }

}
