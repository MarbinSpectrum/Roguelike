using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTile : MonoBehaviour
{
    public TileBundle baseTile;
    public TileData nullTile;

    public virtual TileData GetTileData(TileType[,] pTile)
    {
        return nullTile;
    }
}
