using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTile : MonoBehaviour
{
    public TileData tileData0;
    public TileData tileData1;
    public TileData tileData2;
    public TileData tileData3;
    public TileData tileData4;
    public TileData tileData5;
    public TileData tileData6;
    public TileData tileData7;
    public TileData tileData8;
    public TileData tileData9;
    public TileData tileData10;
    public TileData tileData11;
    public TileData tileData12;
    public TileData tileData13;
    public TileData tileData14;
    public TileData tilefloor;
    public TileData nullTile;


    public TileData GetTileData(TileType[,] pTile)
    {
        if (pTile[1, 1] == TileType.Floor)
            return tilefloor;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[0, 0] == TileType.Floor &&
            pTile[2, 0] == TileType.Floor)
            return tileData13;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[0, 0] == TileType.Floor &&
            pTile[2, 0] == TileType.Floor)
            return tileData13;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Wall &&
            pTile[0, 0] == TileType.Floor &&
            pTile[2, 0] == TileType.Floor)
            return tileData13;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[2, 0] == TileType.Floor)
            return tileData0;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Floor)
            return tileData1;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Floor)
            return tileData1;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Floor)
            return tileData1;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Floor)
            return tileData1;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Floor)
            return tileData1;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[1, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[0, 0] == TileType.Floor)
            return tileData2;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[2, 1] == TileType.Floor)
            return tileData3;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[0, 1] == TileType.Floor)
            return tileData4;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[2, 2] == TileType.Floor)
            return tileData5;
        else if (pTile[1, 2] == TileType.Floor &&
             pTile[0, 1] == TileType.Wall &&
             pTile[2, 1] == TileType.Wall &&
             pTile[1, 0] == TileType.Wall &&
             pTile[2, 0] == TileType.Floor)
            return tileData9;
        else if (pTile[1, 2] == TileType.Floor &&
             pTile[0, 1] == TileType.Wall &&
             pTile[2, 1] == TileType.Wall &&
             pTile[1, 0] == TileType.Wall &&
             pTile[0, 0] == TileType.Floor)
            return tileData8;
        else if (pTile[1, 0] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 2] == TileType.Floor)
            return tileData6;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall &&
            pTile[0, 2] == TileType.Floor)
            return tileData7;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Wall)
            return tileData8;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Wall)
            return tileData9;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Wall &&
            pTile[1, 0] == TileType.Floor)
            return tileData10;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Wall &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Floor)
            return tileData11;
        else if (pTile[1, 2] == TileType.Floor &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Wall)
            return tileData12;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Wall)
            return tileData13;
        else if (pTile[1, 2] == TileType.Wall &&
            pTile[0, 1] == TileType.Floor &&
            pTile[2, 1] == TileType.Floor &&
            pTile[1, 0] == TileType.Floor)
            return tileData14
                ;
        return nullTile;
    }
}
