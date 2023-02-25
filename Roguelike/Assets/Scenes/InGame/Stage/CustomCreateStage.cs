using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : �����ϰ� ���� �����ϴ� �κ�
////////////////////////////////////////////////////////////////////////////////
public class CustomCreateStage : CreateMap
{
    [SerializeField]
    private List<TextAsset> mapChunkData = new List<TextAsset>();

    [SerializeField]
    private uint mapWidth = 5;
    [SerializeField]
    private uint mapHeight = 5;

    protected Dictionary<RoomType1, List<RoomData>> roomDatas 
        = new Dictionary<RoomType1, List<RoomData>>();
    protected Dictionary<RoomType1, List<RoomData>> startRoomDatas
        = new Dictionary<RoomType1, List<RoomData>>();
    protected Dictionary<RoomType1, List<RoomData>> endRoomDatas
        = new Dictionary<RoomType1, List<RoomData>>();
    protected bool[,] IsWall;
    protected Vector2Int startPos;
    protected Vector2Int endPos;


    ////////////////////////////////////////////////////////////////////////////////
    /// : Awake
    ////////////////////////////////////////////////////////////////////////////////
    protected void Awake()
    {
        LoadChunckData();
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : ûũ �����͸� �ε�
    ////////////////////////////////////////////////////////////////////////////////
    protected void LoadChunckData()
    {
        foreach(TextAsset textAsset in mapChunkData)
        {
            RoomData roomData
                = MyLib.Json.JsonToOject<RoomData>(textAsset.text);
            if(roomData.roomType2 == RoomType2.StartRoom)
            {
                if (startRoomDatas.ContainsKey(roomData.roomType1) == false)
                    startRoomDatas[roomData.roomType1] = new List<RoomData>();
                startRoomDatas[roomData.roomType1].Add(roomData);
            }
            else if (roomData.roomType2 == RoomType2.EndRoom)
            {
                if (endRoomDatas.ContainsKey(roomData.roomType1) == false)
                    endRoomDatas[roomData.roomType1] = new List<RoomData>();
                endRoomDatas[roomData.roomType1].Add(roomData);
            }
            else
            {
                if (roomDatas.ContainsKey(roomData.roomType1) == false)
                    roomDatas[roomData.roomType1] = new List<RoomData>();
                roomDatas[roomData.roomType1].Add(roomData);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �̷� ����
    ////////////////////////////////////////////////////////////////////////////////
    protected void MakeMap()
    {
        IsWall = Algorithm.MakeMaze(mapWidth, mapHeight);  //�̷θ� �����Ѵ�.

        List<Vector2Int> wallList = new List<Vector2Int>();
        for (int y = 1; y < (mapHeight * 2 + 1); y++)
        {
            for (int x = (y % 2 == 0 ? 1 : 2); x < (mapWidth * 2 + 1); x += 2)
            {
                if (x >= mapWidth * 2 || y >= mapHeight * 2)
                    continue;
                if (IsWall[x, y] == false)
                    continue;
                //���ֹ��� �� �ĺ� ����
                wallList.Add(new Vector2Int(x, y));
            }
        }

        Algorithm.Shuffle(ref wallList);
        int removeCnt = Mathf.Max((int)mapWidth, (int)mapHeight);
        for (int i = 0; i < removeCnt; i++)
        {
            //������ �� �����
            int x = wallList[i].x;
            int y = wallList[i].y;
            IsWall[x, y] = false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���������� Chunk�迭�� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////

    private ChunkValue[,] GenerateChunkArray()
    {
        ChunkValue[,] mapChucks = null;

        MakeMap();  //�̷θ� �����Ѵ�.

        if (IsWall == null)
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
                bool up = !IsWall[ax,ay + 1];
                bool down = !IsWall[ax, ay - 1];
                bool left = !IsWall[ax - 1, ay];
                bool right = !IsWall[ax + 1, ay];

                if (up == false && down == false && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type0, 0);
                else if (up == true && down == false && left == false && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type0, 1);
                else if (up == false && down == false && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type0, 2);
                else if (up == false && down == true && left == false && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type0, 3);

                else if (up == false && down == false && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type1, 0);
                else if (up == true && down == true && left == false && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type1, 1);

                else if (up == false && down == true && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type2, 0);
                else if (up == true && down == false && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type2, 1);
                else if (up == false && down == true && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type2, 3);
                else if (up == true && down == false && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type2, 2);

                else if (up == true && down == true && left == true && right == false)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type3, 0);
                else if (up == true && down == false && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type3, 1);
                else if (up == true && down == true && left == false && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type3, 2);
                else if (up == false && down == true && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type3, 3);

                else if (up == true && down == true && left == true && right == true)
                    mapChucks[x, y] = new ChunkValue(RoomType1.Type4, 0);
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

    protected void MakeStartPos()
    {
        int startIdx = Random.Range(0, (int)mapWidth * (int)mapHeight);
        startPos = new Vector2Int(startIdx % (int)mapWidth, startIdx / (int)mapWidth);
    }

    protected void MakeEndPos()
    {
        //ť�� ����ؼ� ������������ ���� ���� �ָ������� ��ġ�� ���Ѵ�.

        HashSet<Vector2Int> visit = new HashSet<Vector2Int>();
        //x: �Ÿ� ,y:x��ǥ ,z:y��ǥ
        Queue<Vector3> queue = new Queue<Vector3>();

        Vector2Int startVec = new Vector2Int(startPos.x * 2 + 1, startPos.y * 2 + 1);
        queue.Enqueue(new Vector3(0, startVec.x, startVec.y));
        visit.Add(startVec);

        while(queue.Count > 0)
        {
            Vector3 now = queue.Dequeue();
            endPos = new Vector2Int((int)((now.y - 1) / 2), (int)((now.z - 1) / 2));

            for (int i = 0; i < 4; i++)
            {
                int ax = (int)now.y + Calculator.Around4Pos[i, 0];
                int ay = (int)now.z + Calculator.Around4Pos[i, 1];
                if (ax < 0 || ay < 0 || ax >= IsWall.GetLength(0) || ay >= IsWall.GetLength(1))
                    continue;
                if (IsWall[ax, ay])
                    continue;

                ax = (int)now.y + Calculator.Around4Pos[i, 0] * 2;
                ay = (int)now.z + Calculator.Around4Pos[i, 1] * 2;
                if (ax < 0 || ay < 0 || ax >= IsWall.GetLength(0) || ay >= IsWall.GetLength(1))
                    continue;
                if (IsWall[ax, ay])
                    continue;
                if (visit.Contains(new Vector2Int(ax, ay)))
                    continue;
                queue.Enqueue(new Vector3(now.x+1, ax, ay));
                visit.Add(new Vector2Int(ax, ay));
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Ÿ�ϸ� ����
    ////////////////////////////////////////////////////////////////////////////////
    public override IEnumerator runCreateGameMap()
    {
        ChunkValue[,] mapChucks = GenerateChunkArray();
        if (mapChucks == null)
            yield break;

        tileObjs = new TileObj[mapWidth * RoomData.roomSize, mapHeight * RoomData.roomSize];

        MakeStartPos();
        MakeEndPos();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                bool startRoom = (startPos.x == x && startPos.y == y);
                bool endRoom = (endPos.x == x && endPos.y == y);

                List<RoomData> roomList = 
                    startRoom ? startRoomDatas[mapChucks[x, y].roomType] :
                    endRoom ? endRoomDatas[mapChucks[x, y].roomType] :
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
                        newTileObj.transform.parent = transform;
                        newTileObj.isRun = false;

                        SetTileType(i, j, RoomData.roomSize, RoomData.roomSize, ref newTileObj, ref tileType);

                        tileObjs[ax, ay] = newTileObj;                     
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
                        SetObj(obj[i, j], ax, ay);                      
                    }
                }

                yield return new WaitForSeconds(0.001f);
            }
        }
    }
}
