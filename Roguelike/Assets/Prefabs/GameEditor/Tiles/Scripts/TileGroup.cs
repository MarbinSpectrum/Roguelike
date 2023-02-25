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
    [SerializeField]
    private EditorTileObj tilePrefabs;
    public RoomEditor roomEditor;
    public CustomMapEditor customMapEditor;
    private uint w;
    private uint h;

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateTileGroup();
    }

    public void UpdateTileGroup()
    {
        if (roomEditor != null)
        {
            w = RoomData.roomSize;
            h = RoomData.roomSize;
        }
        else if (customMapEditor != null)
        {
            w = customMapEditor.width;
            h = customMapEditor.height;
        }
        else
        {
            return;
        }

        EditorTileObj[,] tiles = new EditorTileObj[w, h];

        TileType GetTileType(int x, int y)
        {
            if (x < 0 || y < 0 || x >= w || y >= h)
                return TileType.Wall;
            if (tiles[x, y] == null)
                return TileType.Wall;
            return tiles[x, y].tileType;
        }

        int idx = 0;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (tileObjs.Count <= idx)
                {
                    EditorTileObj newTile = Instantiate(tilePrefabs);
                    tileObjs.Add(newTile);
                }
                EditorTileObj eTileObj = tileObjs[idx];
                if (eTileObj == null)
                    continue;
                eTileObj.gameObject.SetActive(true);
                eTileObj.transform.position
                    = new Vector3(
                        startPos.x + x * tileWidth,
                        startPos.y + y * tileHeight, 0);
                eTileObj.transform.parent = transform;
                eTileObj.transform.localScale = new Vector3(1, 1, 1);
                tiles[x, y] = eTileObj;
                idx++;
            }
        }

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                List<Vector2Int> aroundPos = Calculator.GetAround8Pos(x, y);

                TileType[,] tileTypes = new TileType[3, 3];
                foreach (Vector2Int pos in aroundPos)
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
        if (roomEditor != null)
        {
            w = RoomData.roomSize;
            h = RoomData.roomSize;
        }
        else if (customMapEditor != null)
        {
            w = customMapEditor.width;
            h = customMapEditor.height;
        }
        else
        {
            return null;
        }

        List<TileType> tiles = new List<TileType>();

        int idx = 0;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : 맵을 초기화한다.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Clear", ButtonSizes.Large)]
    public void ExportData()
    {
        foreach (EditorTileObj editorTileObj in tileObjs)
        {
            editorTileObj.tileType = TileType.Wall;
        }
    }
}
