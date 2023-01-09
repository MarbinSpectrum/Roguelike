using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 타일의 그룹을 관리합니다.
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class TileGroup : SerializedMonoBehaviour
{
    [HideInInspector]
    public float tileWidth;
    [HideInInspector]
    public float tileHeight;
    [HideInInspector]
    public Vector2 startPos;

    [Title("GetTile")]
    [SerializeField]
    private GetTile getTile;

    [Title("RequireData")]
    [SerializeField]
    private List<EditorTileObj> tileObjs = new List<EditorTileObj>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        EditorTileObj[,] tiles = new EditorTileObj[RoomData.roomSize, RoomData.roomSize];

        TileType GetTileType(int x,int y)
        {
            if (x < 0 || y < 0 || x >= RoomData.roomSize || y >= RoomData.roomSize)
                return TileType.Wall;
            if (tiles[x, y] == null)
                return TileType.Wall;
            return tiles[x, y].tileType;
        }

        int idx = 0;
        for(int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                if (tileObjs.Count <= idx)
                    continue;
                EditorTileObj eTileObj = tileObjs[idx];
                if (eTileObj == null)
                    continue;
                eTileObj.gameObject.SetActive(true);
                eTileObj.transform.position
                    = new Vector3(
                        startPos.x + x * tileWidth, 
                        startPos.y + y * tileHeight, 0);
                tiles[x, y] = eTileObj;
                idx++;
            }
        }

        for (int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                List<Vector2Int> aroundPos = Calculator.GetAround8Pos(x, y);

                TileType[,] tileTypes = new TileType[3, 3];
                foreach(Vector2Int pos in aroundPos)
                {
                    tileTypes[pos.x - x + 1, pos.y - y + 1] = GetTileType(pos.x, pos.y);
                }
                tileTypes[1, 1] = tiles[x, y].tileType;

                TileData tileData = getTile.GetTileData(tileTypes);

                tiles[x, y].SetSprite(tileData);
            }
        }

        for (; idx < tileObjs.Count; idx++)
        {
            EditorTileObj eTileObj = tileObjs[idx];
            if (eTileObj == null)
                continue;
            eTileObj.gameObject.SetActive(false);
        }
    }

    public List<TileType> GetTiles()
    {
        List<TileType> tiles = new List<TileType>();

        int idx = 0;
        for (int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                if (tileObjs.Count <= idx)
                    continue;
                EditorTileObj eTileObj = tileObjs[idx];
                if (eTileObj == null)
                    continue;
                tiles.Add(eTileObj.tileType);

                idx++;
            }
        }

        return tiles;
    }
}
