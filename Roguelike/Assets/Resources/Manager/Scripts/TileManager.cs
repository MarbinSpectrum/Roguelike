using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 타일의 정보를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class TileManager : SerializedMonoBehaviour
{
    private Dictionary<Tile, TileData> tileDatas;

    [SerializeField]
    private List<TileData> tileDataList = new List<TileData>();

    private void Init()
    {
        if(tileDatas == null)
        {
            tileDatas = new Dictionary<Tile, TileData>();
            foreach (TileData tileData in tileDataList)
            {
                tileDatas[tileData.tile] = tileData;
            }
        }

    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pTile에 해당하는 타일의 Sprite를 반환한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Sprite GetSprite(Tile pTile)
    {
        Init();

        TileData tileData = tileDatas[pTile];
        Sprite tileSprite = tileData.tileSprite;

        return tileSprite;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pTile에 해당하는 타일의 TileType를 반환한다.
    ////////////////////////////////////////////////////////////////////////////////
    public TileType GetType(Tile pTile)
    {
        Init();

        TileData tileData = tileDatas[pTile];
        TileType tileType = tileData.tileType;

        return tileType;
    }

    public static bool IsFloor(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Stone_Floor:
            case TileType.Stone_Stair:
            case TileType.Mat_Floor:
            case TileType.Wood_Floor:
            case TileType.Concrete_Floor:
                return true;
        }
        return false;
    }
}
