using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : ûũ ���� �ǹ��Ѵ�. //�� ���� //ȸ����
////////////////////////////////////////////////////////////////////////////////
public struct ChunkValue
{
    public RoomType roomType;
    public uint rotate;
    public ChunkValue(RoomType pRoomType, uint pRotate)
    {
        roomType = pRoomType;
        rotate = pRotate;       
    }
}

public struct MapMonster
{
    public Obj monsterType;
    public Vector2Int pos;
    public MapMonster(Obj pMonsterType, int pX,int pY)
    {
        monsterType = pMonsterType;
        pos = new Vector2Int(pX,pY);
    }

}

////////////////////////////////////////////////////////////////////////////////
/// : �����ϰ� ���� �����ϴ� �κ�
////////////////////////////////////////////////////////////////////////////////
public class CreateMap : MonoBehaviour
{
    public const float tileSize = 2.46f;

    [SerializeField]
    private List<TextAsset> mapChunkData = new List<TextAsset>();

    [SerializeField]
    private GetTile getTile;

    [SerializeField]
    private uint mapWidth = 5;
    [SerializeField]
    private uint mapHeight = 5;

    [SerializeField]
    private TileObj tileObj;

    private Dictionary<RoomType, List<RoomData>> roomDatas 
        = new Dictionary<RoomType, List<RoomData>>();
    private Dictionary<RoomType, List<RoomData>> startRoomDatas
    = new Dictionary<RoomType, List<RoomData>>();

    [HideInInspector]
    public TileObj[,] tileObjs;
    [HideInInspector]
    public List<Vector2Int> startPosList = new List<Vector2Int>();
    [HideInInspector]
    public List<MapMonster> monsterList = new List<MapMonster>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : Awake
    ////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        LoadChunckData();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ûũ �����͸� �ε�
    ////////////////////////////////////////////////////////////////////////////////
    private void LoadChunckData()
    {
        foreach(TextAsset textAsset in mapChunkData)
        {
            RoomData roomData
                = MyLib.Json.JsonToOject<RoomData>(textAsset.text);
            if(roomData.isStartRoom)
            {
                if (startRoomDatas.ContainsKey(roomData.roomType) == false)
                    startRoomDatas[roomData.roomType] = new List<RoomData>();
                startRoomDatas[roomData.roomType].Add(roomData);
            }
            else
            {
                if (roomDatas.ContainsKey(roomData.roomType) == false)
                    roomDatas[roomData.roomType] = new List<RoomData>();
                roomDatas[roomData.roomType].Add(roomData);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���������� Chunk�迭�� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////

    private ChunkValue[,] GenerateChunkArray()
    {
        ChunkValue[,] mapChucks = null;

        bool[,] isWall = Algorithm.MakeMaze(mapWidth, mapHeight);  //�̷θ� �����Ѵ�.

        if (isWall == null)
            return mapChucks;

        mapChucks = new ChunkValue[mapWidth, mapHeight];   //��� ���� ûũ�� ��ġ�ϱ����� �迭

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                //���� ���� ���ο� ���� �����ؾ��ϴ�
                //Ÿ���� ûũ�� �ٸ���.
                //��� ��츦 ����ؼ� ûũ�� ��ġ�Ѵ�.

                int ax = x * 2 + 1;
                int ay = y * 2 + 1;
                bool up = !isWall[ax,ay + 1];
                bool down = !isWall[ax, ay - 1];
                bool left = !isWall[ax - 1, ay];
                bool right = !isWall[ax + 1, ay];

                if (up == false && down == false && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type0, 0);
                else if (up == true && down == false && left == false && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type0, 1);
                else if (up == false && down == false && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type0, 2);
                else if (up == false && down == true && left == false && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type0, 3);

                else if (up == false && down == false && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type1, 0);
                else if (up == true && down == true && left == false && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type1, 1);

                else if (up == false && down == true && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type2, 0);
                else if (up == true && down == false && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type2, 1);
                else if (up == false && down == true && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type2, 3);
                else if (up == true && down == false && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type2, 2);

                else if (up == true && down == true && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type3, 0);
                else if (up == true && down == false && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type3, 1);
                else if (up == true && down == true && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type3, 2);
                else if (up == false && down == true && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type3, 3);

                else if (up == true && down == true && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType.Type4, 0);
            }
        }

        return mapChucks;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ûũ���� �˸´� ������ ������ ��ȯ
    ////////////////////////////////////////////////////////////////////////////////
    private TileType[,] GetChunkTile(ChunkValue pChunkValue, RoomData pRoomData)
    {
        List<TileType> tileTypes = pRoomData.tiles;

        //Ÿ�ϰ��� �迭�� ��´�.
        TileType[,] tileBoard = new TileType[RoomData.roomSize, RoomData.roomSize];
        int idx = 0;
        for (int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                tileBoard[x, y] = tileTypes[idx];
                idx++;
            }
        }

        for(int i = 0; i < pChunkValue.rotate; i++)
        {
            //ȸ������ŭ ûũ�� ȸ�����ִ� �κ�
            TileType[,] tempTileBoard = new TileType[RoomData.roomSize, RoomData.roomSize];
            for (int x = 0; x < RoomData.roomSize; x++)
                for (int y = 0; y < RoomData.roomSize; y++)
                    tempTileBoard[y, RoomData.roomSize - x - 1] = tileBoard[x, y];

            for (int x = 0; x < RoomData.roomSize; x++)
                for (int y = 0; y < RoomData.roomSize; y++)
                    tileBoard[x, y] = tempTileBoard[x, y];
        }

        return tileBoard;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ûũ���� �˸´� ������ ������Ʈ�� ��ȯ
    ////////////////////////////////////////////////////////////////////////////////
    private Obj[,] GetChunkObj(ChunkValue pChunkValue, RoomData pRoomData)
    {
        List<Obj> objs = pRoomData.objs;

        //Ÿ�ϰ��� �迭�� ��´�.
        Obj[,] objBoard = new Obj[RoomData.roomSize, RoomData.roomSize];
        int idx = 0;
        for (int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                objBoard[x, y] = objs[idx];
                idx++;
            }
        }

        for (int i = 0; i < pChunkValue.rotate; i++)
        {
            //ȸ������ŭ ûũ�� ȸ�����ִ� �κ�
            Obj[,] tempObjBoard = new Obj[RoomData.roomSize, RoomData.roomSize];
            for (int x = 0; x < RoomData.roomSize; x++)
                for (int y = 0; y < RoomData.roomSize; y++)
                    tempObjBoard[y, RoomData.roomSize - x - 1] = objBoard[x, y];

            for (int x = 0; x < RoomData.roomSize; x++)
                for (int y = 0; y < RoomData.roomSize; y++)
                    objBoard[x, y] = tempObjBoard[x, y];
        }

        return objBoard;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Ÿ�ϸ� ����
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateGameMap()
    {
        ChunkValue[,] mapChucks = GenerateChunkArray();
        if (mapChucks == null)
            yield break;

        tileObjs = new TileObj[mapWidth * RoomData.roomSize, mapHeight * RoomData.roomSize];

        int startIdx = Random.Range(0, (int)mapWidth * (int)mapHeight);
        Vector2Int startPos = 
            new Vector2Int(startIdx% (int)mapWidth, startIdx / (int)mapWidth);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                bool startRoom = (startPos.x == x && startPos.y == y);

                List<RoomData> roomList = startRoom ?
                    startRoomDatas[mapChucks[x, y].roomType] :
                    roomDatas[mapChucks[x, y].roomType];

                int dataSize = roomList.Count;
                RoomData roomData = roomList[Random.Range(0, dataSize)];

                TileType[,] tileType = GetChunkTile(mapChucks[x, y], roomData);
                //�ش� ûũ�� �ش��ϴ� ����� �����Ѵ�.
                for (int i = 0; i < RoomData.roomSize; i++)
                {
                    for (int j = 0; j < RoomData.roomSize; j++)
                    {
                        int ax = x * (int)RoomData.roomSize + i;
                        int ay = y * (int)RoomData.roomSize + j;

                        TileObj newTileObj = Instantiate(tileObj);
                        newTileObj.transform.position = new Vector3(ax * tileSize, ay * tileSize, 0);

                        //�ֺ� ����� �ǵ��ؼ� � ����� ����� ������ ���Ѵ�.
                        //�ֺ� 8���� ����� �����´�.
                        List<Vector2Int> aroundPos = Calculator.GetAround8Pos(i, j);
                        TileType[,] aroundTile = new TileType[3, 3];
                        foreach (Vector2Int pos in aroundPos)
                        {
                            int aroundX = pos.x - i + 1;
                            int aroundY = pos.y - j + 1;
                            if (pos.x < 0 || pos.y < 0
                                || pos.x >= RoomData.roomSize || pos.y >= RoomData.roomSize)
                                aroundTile[aroundX, aroundY] = TileType.Wall;
                            else
                                aroundTile[aroundX, aroundY] = tileType[pos.x, pos.y];
                        }
                        aroundTile[1, 1] = tileType[i, j];

                        //�˸´� Ÿ�� ����� �����Ѵ�.
                        TileData tileData = getTile.GetTileData(aroundTile);
                        newTileObj.isTile = tileData.tile;

                        tileObjs[ax, ay] = newTileObj;

                        newTileObj.transform.parent = transform;
                        newTileObj.isRun = false;
                    }
                }

                Obj[,] obj = GetChunkObj(mapChucks[x, y], roomData);
                //�ش� ûũ�� �ش��ϴ� ������Ʈ��
                //�����Ѵ�.
                for (int i = 0; i < RoomData.roomSize; i++)
                {
                    for (int j = 0; j < RoomData.roomSize; j++)
                    {
                        int ax = x * (int)RoomData.roomSize + i;
                        int ay = y * (int)RoomData.roomSize + j;

                        switch(obj[i, j])
                        {
                            case Obj.Null:
                                break;
                            case Obj.StartPos:
                                {
                                    startPosList.Add(new Vector2Int(ax, ay));
                                }
                                break;
                            case Obj.TorchLight:
                                {
                                    TorchManager torchManager = TorchManager.instance;
                                    torchManager.AddTorchPos(new Vector2Int(ax, ay));
                                }
                                break;
                                //���� ��ü ���
                            case Obj.Slime:
                            case Obj.Tentacle:
                                {
                                    monsterList.Add(new MapMonster(obj[i, j], ax, ay));
                                }
                                break;
                        }

                    }
                }

                yield return new WaitForSeconds(0.001f);
            }
        }
    }
}
