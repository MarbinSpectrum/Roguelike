using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBundle : MonoBehaviour
{
    public virtual TileData GetTileData(TileType[,] pTile)
    {
        return null;
    }
}
