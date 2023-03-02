using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatTileBundle : TileBundle
{
    public List<TileData> tileDatas;
    public TileData nullTile;

    public override TileData GetTileData(TileType[,] pTile)
    {
        if (pTile[1, 2] != TileType.Mat_Floor &&
           pTile[0, 1] != TileType.Mat_Floor &&
            pTile[1, 0] == TileType.Mat_Floor &&
             pTile[2, 0] == TileType.Mat_Floor &&
              pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[0];
        else if (pTile[1, 2] != TileType.Mat_Floor &&
                    pTile[0, 0] == TileType.Mat_Floor &&
        pTile[2, 0] == TileType.Mat_Floor &&
        pTile[0, 1] == TileType.Mat_Floor &&
         pTile[1, 0] == TileType.Mat_Floor &&
          pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[1];
        else if (pTile[1, 2] != TileType.Mat_Floor &&
           pTile[2, 1] != TileType.Mat_Floor &&
            pTile[1, 0] == TileType.Mat_Floor &&
             pTile[0, 0] == TileType.Mat_Floor &&
              pTile[0, 1] == TileType.Mat_Floor)
            return tileDatas[2];
        else if (pTile[0, 1]!= TileType.Mat_Floor &&
            pTile[2, 2] == TileType.Mat_Floor &&
            pTile[2, 0] == TileType.Mat_Floor &&
    pTile[1, 2] == TileType.Mat_Floor &&
     pTile[2, 1] == TileType.Mat_Floor &&
      pTile[1, 0] == TileType.Mat_Floor)
            return tileDatas[3];
        else if (pTile[0, 2] == TileType.Mat_Floor &&
            pTile[2, 2] == TileType.Mat_Floor &&
            pTile[0, 0] == TileType.Mat_Floor &&
            pTile[2, 0] == TileType.Mat_Floor &&
            pTile[0, 1] == TileType.Mat_Floor &&
            pTile[1, 2] == TileType.Mat_Floor &&
            pTile[2, 1] == TileType.Mat_Floor &&
            pTile[1, 0] == TileType.Mat_Floor)
            return tileDatas[4];
        else if (pTile[2, 1] != TileType.Mat_Floor &&
            pTile[0, 2] == TileType.Mat_Floor &&
            pTile[0, 0] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor)
            return tileDatas[5];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
   pTile[0, 1] != TileType.Mat_Floor &&
    pTile[2, 1] == TileType.Mat_Floor &&
     pTile[1, 2] == TileType.Mat_Floor &&
      pTile[2, 2] == TileType.Mat_Floor)
            return tileDatas[6];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[0, 2] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[7];
        else if (pTile[2, 1] != TileType.Mat_Floor &&
pTile[1, 0] != TileType.Mat_Floor &&
pTile[0, 2] == TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor)
            return tileDatas[8];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[9];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[10];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor)
            return tileDatas[11];
        else if (pTile[1, 0] == TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor)
            return tileDatas[12];
        else if (pTile[1, 0] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor)
            return tileDatas[13];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor)
            return tileDatas[14];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor)
            return tileDatas[15];
        else if (pTile[1, 0] == TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[2, 0] != TileType.Mat_Floor)
            return tileDatas[16];
        else if (pTile[1, 0] == TileType.Mat_Floor &&
pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor &&
pTile[0, 0] != TileType.Mat_Floor)
            return tileDatas[17];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[0, 1] != TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[2, 2] != TileType.Mat_Floor)
            return tileDatas[18];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] != TileType.Mat_Floor &&
pTile[0, 2] != TileType.Mat_Floor)
            return tileDatas[19];
        else if (pTile[1, 2] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[0, 0] != TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor &&
pTile[2, 0] != TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[20];
        else if (pTile[2, 1] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[0, 2] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[0, 0] != TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor)
            return tileDatas[21];
        else if (pTile[0, 1] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] != TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[2, 0] != TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor)
            return tileDatas[22];
        else if (pTile[1, 0] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[0, 2] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] != TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor)
            return tileDatas[23];
        else if (pTile[0, 0] != TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor &&
pTile[2, 0] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[0, 2] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] != TileType.Mat_Floor)
            return tileDatas[24];
        else if (pTile[0, 0] == TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor &&
pTile[2, 0] != TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[0, 2] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] == TileType.Mat_Floor)
            return tileDatas[25];
        else if (pTile[0, 0] != TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor &&
pTile[2, 0] == TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[0, 2] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] == TileType.Mat_Floor)
            return tileDatas[26];
        else if (pTile[0, 0] == TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor &&
pTile[2, 0] == TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[0, 2] == TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] != TileType.Mat_Floor)
            return tileDatas[27];
        else if (pTile[0, 0] == TileType.Mat_Floor &&
pTile[1, 0] == TileType.Mat_Floor &&
pTile[2, 0] == TileType.Mat_Floor &&
pTile[0, 1] == TileType.Mat_Floor &&
pTile[2, 1] == TileType.Mat_Floor &&
pTile[0, 2] != TileType.Mat_Floor &&
pTile[1, 2] == TileType.Mat_Floor &&
pTile[2, 2] == TileType.Mat_Floor)
            return tileDatas[28];
        return nullTile;
    }
}
