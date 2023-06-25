using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

////////////////////////////////////////////////////////////////////////////////
/// : 랜덤하게 맵을 생성하는 부분
////////////////////////////////////////////////////////////////////////////////
public class CreateRandomMap : CreateMap
{
    [SerializeField] private MapType mapType;
    private uint mapWidth;
    private uint mapHeight;
    private TextAsset[] mapDatas;

    private TileType[,] GetTileTypes(CustomMapData pCustomMapData)
    {
        TileType[,] tileType = new TileType[pCustomMapData.roomW, pCustomMapData.roomH];
        List<TileType> tiles = pCustomMapData.tiles;

        for (uint idx = 0; idx < tiles.Count; idx++)
        {
            uint x = idx / pCustomMapData.roomH;
            uint y = idx % pCustomMapData.roomH;
            tileType[x, y] = tiles[(int)idx];
        }

        return tileType;
    }

    private Obj[,] GetObjs(CustomMapData pCustomMapData)
    {
        Obj[,] Objs = new Obj[pCustomMapData.roomW, pCustomMapData.roomH];
        List<Obj> objs = pCustomMapData.objs;

        for (uint idx = 0; idx < objs.Count; idx++)
        {
            uint x = idx / pCustomMapData.roomH;
            uint y = idx % pCustomMapData.roomH;
            Objs[x, y] = objs[(int)idx];
        }

        return Objs;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 타일맵 생성 // pVisitRooms에 포함되지 않은 방만 출현되도록 구현
    ////////////////////////////////////////////////////////////////////////////////
    public override IEnumerator runCreateGameMap()
    {
        MapManager mapManager = MapManager.instance;
        List<int> visitRooms = mapManager.GetVisitRoomIdx();


        mapDatas = Resources.LoadAll<TextAsset>("MapDatas/" + mapType.ToString());

        
        List<int> roomIdx = new List<int>();
        for(int i = 0; i < mapDatas.Length; i++)
        {
            if (visitRooms.Contains(i))
                continue;
            roomIdx.Add(i);
        }

        int randomIdx = Random.Range(0, roomIdx.Count);
        int mapIdx = roomIdx[randomIdx];

        mapManager.SetNowRoomIdx(mapIdx);
        CustomMapData customMapData = MyLib.Json.JsonToOject<CustomMapData>(mapDatas[mapIdx].text);

        if (customMapData == null)
            yield break;


        mapWidth = customMapData.roomW;
        mapHeight = customMapData.roomH;
        tileObjs = new TileObj[mapWidth, mapHeight];
        TileType[,] tileType = GetTileTypes(customMapData);
        Obj[,] obj = GetObjs(customMapData);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                TileObj newTileObj = Instantiate(tileObj);
                newTileObj.transform.position = new Vector3(x * tileSize, y * tileSize, 0);
                newTileObj.transform.parent = transform;
                newTileObj.isRun = false;

                SetTileType(x, y, mapWidth, mapHeight, ref newTileObj, ref tileType);
                tileObjs[x, y] = newTileObj;

                SetObj(obj[x, y], x, y);
            }
        }
    }
}
