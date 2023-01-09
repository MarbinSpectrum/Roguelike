using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : Ÿ���� ������ �����ϴ� �Ŵ���
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
    /// : pTile�� �ش��ϴ� Ÿ���� Sprite�� ��ȯ�Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Sprite GetSprite(Tile pTile)
    {
        Init();

        TileData tileData = tileDatas[pTile];
        Sprite tileSprite = tileData.tileSprite;

        return tileSprite;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pTile�� �ش��ϴ� Ÿ���� TileType�� ��ȯ�Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public TileType GetType(Tile pTile)
    {
        Init();

        TileData tileData = tileDatas[pTile];
        TileType tileType = tileData.tileType;

        return tileType;
    }
}
