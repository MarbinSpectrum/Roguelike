using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : ���� �����ϴ� �Ŵ����Դϴ�.
////////////////////////////////////////////////////////////////////////////////
public class MapManager : FieldObjectSingleton<MapManager>
{
    private bool[,] isActCheck;
    private int ArrayW;
    public int arrayW
    {
        get { return ArrayW; }
    }
    private int ArrayH;
    public int arrayH
    {
        get { return ArrayH; }
    }


    [Title("���� �̸� Ű �� / �� ����� ���̸� ���̴� ����")]
    public string mapNameKey;
    public bool showName = true;

    [Title("actSize��ŭ�� ũ���� �ֺ���ϸ� Ȱ��ȭ�ȴ�.")]
    [SerializeField] private int actSize;

    [Space(30)]
    [Title("-------------------------------------------------------------")]
    [SerializeField] private CreateMap createMap;
    [SerializeField] private MiniMap miniMap;
    private TileObj[,] tiles;
    private HashSet<Vector2Int> cantMovePos;
    private List<int> visitRoomIdx = new List<int>();
    private int nowStageIdx;

    ////////////////////////////////////////////////////////////////////////////////
    /// : Start
    ////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        gameMgr.StartGame();
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : Ÿ�ϸ��� �����Ѵ�. // pVisitRooms�� ���Ե��� ���� �游 �����ǵ��� ����
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateTileMap()
    {
        yield return createMap.runCreateGameMap();

        tiles = createMap.tileObjs;
        cantMovePos = createMap.cantMovePos;

        ArrayW = tiles.GetLength(0);
        ArrayH = tiles.GetLength(1);
        isActCheck = new bool[arrayW, arrayH];

        yield return miniMap.runCreateMiniMap();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش���ġ(x,y)�ֺ��� actSize��ŭ�� ��ϸ� Ȱ��ȭ ��Ų��.
    ////////////////////////////////////////////////////////////////////////////////
    public void ActAreaTile(int x,int y)
    {
        //��ü Ÿ�� ��Ȱ��ȭ
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
                isActCheck[i, j] = false;

        //x,y �ֺ� Ÿ�� Ȱ��ȭ
        for (int ax = x - actSize; ax <= x + actSize; ax++)
        {
            for (int ay = y - actSize; ay <= y + actSize; ay++)
            {
                if (ax < 0 || ay < 0 || ax >= arrayW || ay >= arrayH)
                    continue;
                isActCheck[ax, ay] = true;
            }
        }

        //�������� Ȱ��ȭ
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
                tiles[i, j].isRun = isActCheck[i, j];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���������� ������ �ϳ��� ��ǥ�� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetRandomStartPos()
    {
        List<Vector2Int> startPosList = createMap.startPosList;
        int random = Random.Range(0, startPosList.Count);
        return startPosList[random];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �������� ��ǥ�� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetEndPos()
    {
        if (createMap.gameEndPos.Count == 0)
            return new Vector2Int(-1, -1);
        return createMap.gameEndPos[0];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ѱ� �۾����� ��ǥ�� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetGunBenchPos()
    {
        if (createMap.gunBenchPos.Count == 0)
            return new Vector2Int(-1, -1);
        return createMap.gunBenchPos[0];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ ��ǥ�� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetShopPos()
    {
        if (createMap.shopPos.Count == 0)
            return new Vector2Int(-1, -1);
        return createMap.shopPos[0];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �̵� �Ұ��� �������� üũ
    ////////////////////////////////////////////////////////////////////////////////
    public bool CantMovePos(Vector2Int pPos)
    {
        if(cantMovePos.Contains(pPos))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ѱ� �۾��뿡�� �̵� ����
    ////////////////////////////////////////////////////////////////////////////////
    public bool CanMoveGunBench(Vector2Int pFrom,Vector2Int pTo)
    {
        Vector2Int gPos = GetGunBenchPos();
        if (gPos != new Vector2Int(-1, -1))
        {
            if (pFrom == new Vector2Int(gPos.x - 1, gPos.y) &&
                pTo == new Vector2Int(gPos.x - 1, gPos.y + 1))
                return false;
            if (pFrom == gPos &&
                pTo == new Vector2Int(gPos.x, gPos.y + 1))
                return false;
            if (pFrom == new Vector2Int(gPos.x + 1, gPos.y) &&
                pTo == new Vector2Int(gPos.x + 1, gPos.y + 1))
                return false;
            if (pFrom == new Vector2Int(gPos.x - 1, gPos.y + 1) &&
                pTo == new Vector2Int(gPos.x - 1, gPos.y))
                return false;
            if (pFrom == new Vector2Int(gPos.x, gPos.y + 1) &&
                pTo == gPos)
                return false;
            if (pFrom == new Vector2Int(gPos.x + 1, gPos.y + 1) &&
                pTo == new Vector2Int(gPos.x + 1, gPos.y))
                return false;
        }
        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ѱ� �۾��뿡�� �̵� ����
    ////////////////////////////////////////////////////////////////////////////////
    public bool CanMoveShop(Vector2Int pFrom, Vector2Int pTo)
    {
        Vector2Int gPos = GetShopPos();
        //if (GameManager.gunBenchAct)
        {
            if (gPos != new Vector2Int(-1, -1))
            {
                if (pFrom == new Vector2Int(gPos.x, gPos.y + 2) &&
                    pTo == new Vector2Int(gPos.x - 1, gPos.y + 2))
                    return false;
                if (pFrom == new Vector2Int(gPos.x, gPos.y + 1) &&
                    pTo == new Vector2Int(gPos.x - 1, gPos.y + 1))
                    return false;
                if (pFrom == gPos &&
                    pTo == new Vector2Int(gPos.x - 1, gPos.y))
                    return false;
                if (pFrom == new Vector2Int(gPos.x, gPos.y - 1) &&
                    pTo == new Vector2Int(gPos.x - 1, gPos.y - 1))
                    return false;
                if (pFrom == new Vector2Int(gPos.x - 1, gPos.y + 2) &&
                    pTo == new Vector2Int(gPos.x, gPos.y + 2))
                    return false;
                if (pFrom == new Vector2Int(gPos.x - 1, gPos.y + 1) &&
                    pTo == new Vector2Int(gPos.x, gPos.y + 1))
                    return false;
                if (pFrom == new Vector2Int(gPos.x - 1, gPos.y) &&
                    pTo == gPos)
                    return false;
                if (pFrom == new Vector2Int(gPos.x - 1, gPos.y - 1) &&
                    pTo == new Vector2Int(gPos.x, gPos.y - 1))
                    return false;
            }
        }
        //else
        //{
        //    if (gPos != new Vector2Int(-1, -1))
        //    {
        //        if (pTo == new Vector2Int(gPos.x - 1, gPos.y))
        //            return false;
        //        if (pTo == new Vector2Int(gPos.x, gPos.y))
        //            return false;
        //        if (pTo == new Vector2Int(gPos.x + 1, gPos.y))
        //            return false;
        //    }
        //}
        return true;
    }

    //////////////////////////////////////////////////////////////////
    /// : �ʿ� �ִ� ���͸� �����´�.
    ////////////////////////////////////////////////////////////////////////////////
    public List<MapMonster> GetMonsterList()
    {
        List<MapMonster> monsterList = createMap.monsterList;
        return monsterList;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش���ġ(x,y)�� ������ �ǵ��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsWall(int x,int y)
    {
        if (x < 0 || y < 0 || x >= arrayW || y >= arrayH)
            return true;
        if (tiles[x, y] == null)
            return true;
        if (TileManager.IsFloor(tiles[x, y].tileType) == false)
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش���ġ(x,y)�� �ܰ������� �ǵ��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsEdge(int x, int y)
    {
        if (x < 0 || y < 0 || x >= arrayW || y >= arrayH)
            return false;
        if (tiles[x, y] == null)
            return false;
        if (tiles[x, y].isTile != Tile.Null_Tile && 
            TileManager.IsFloor(tiles[x, y].tileType) == false)
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos�� �̴ϸ��� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateMiniMap(Vector2Int pPos)
    {
        miniMap.UpdateMiniMapPos(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �̴ϸ� �ؽ�ó�� �޾ƿ´�.
    ////////////////////////////////////////////////////////////////////////////////
    public Texture2D GetMiniMapTexture()
    {
        return miniMap.GetMiniMapTexture();
    }

    public void ShowMapName()
    {
        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowMapName(LanguageManager.GetText(mapNameKey), showName);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �湮�� ���� ����Ʈ�� �������ų� �ʱ�ȭ�Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public List<int> GetVisitRoomIdx()
    {
        return visitRoomIdx;
    }

    public void SetVisitRoomIdx(List<int> pList)
    {
        visitRoomIdx = new List<int>(pList);
    }

    public void AddVisitRoomIdx(int pRoomIdx)
    {
        if (visitRoomIdx.Contains(pRoomIdx))
            return;
        visitRoomIdx.Add(pRoomIdx);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� �ε����� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////

    public int GetNowRoomIdx()
    {
        return nowStageIdx;
    }
    public void SetNowRoomIdx(int pRoomIdx)
    {
        nowStageIdx = pRoomIdx;
    }
}
