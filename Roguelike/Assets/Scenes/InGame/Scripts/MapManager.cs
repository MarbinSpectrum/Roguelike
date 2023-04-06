using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 맵을 관리하는 매니저입니다.
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

    public bool showName = true;
    public string mapNameKey;

    [SerializeField]
    private CreateMap createMap;
    [SerializeField]
    private MiniMap miniMap;

    [SerializeField]
    private int actSize;

    private TileObj[,] tiles;
    private HashSet<Vector2Int> cantMovePos;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 타일맵을 생성한다.
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
    /// : 해당위치(x,y)주변에 actSize만큼의 블록만 활성화 시킨다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ActAreaTile(int x,int y)
    {
        //전체 타일 비활성화
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
                isActCheck[i, j] = false;

        //x,y 주변 타일 활성화
        for (int ax = x - actSize; ax <= x + actSize; ax++)
        {
            for (int ay = y - actSize; ay <= y + actSize; ay++)
            {
                if (ax < 0 || ay < 0 || ax >= arrayW || ay >= arrayH)
                    continue;
                isActCheck[ax, ay] = true;
            }
        }

        //실질적인 활성화
        for (int i = 0; i < arrayW; i++)
            for (int j = 0; j < arrayH; j++)
                tiles[i, j].isRun = isActCheck[i, j];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 시작지점중 랜덤한 하나의 좌표를 출력한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetRandomStartPos()
    {
        List<Vector2Int> startPosList = createMap.startPosList;
        int random = Random.Range(0, startPosList.Count);
        return startPosList[random];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 도착지점 좌표를 출력한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetEndPos()
    {
        if (createMap.gameEndPos.Count == 0)
            return new Vector2Int(-1, -1);
        return createMap.gameEndPos[0];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총기 작업대의 좌표를 출력한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetGunBenchPos()
    {
        if (createMap.gunBenchPos.Count == 0)
            return new Vector2Int(-1, -1);
        return createMap.gunBenchPos[0];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 상점의 좌표를 출력한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetShopPos()
    {
        if (createMap.shopPos.Count == 0)
            return new Vector2Int(-1, -1);
        return createMap.shopPos[0];
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 이동 불가능 지역인지 체크
    ////////////////////////////////////////////////////////////////////////////////
    public bool CantMovePos(Vector2Int pPos)
    {
        if(cantMovePos.Contains(pPos))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총기 작업대에서 이동 제한
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
    /// : 총기 작업대에서 이동 제한
    ////////////////////////////////////////////////////////////////////////////////
    public bool CanMoveShop(Vector2Int pFrom, Vector2Int pTo)
    {
        Vector2Int gPos = GetShopPos();
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
            if (pFrom == new Vector2Int(gPos.x- 1, gPos.y + 2) &&
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
        return true;
    }

    //////////////////////////////////////////////////////////////////
    /// : 맵에 있는 몬스터를 가져온다.
    ////////////////////////////////////////////////////////////////////////////////
    public List<MapMonster> GetMonsterList()
    {
        List<MapMonster> monsterList = createMap.monsterList;
        return monsterList;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 해당위치(x,y)가 벽인지 판독한다.
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
    /// : 해당위치(x,y)가 외각벽인지 판독한다.
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
    /// : pPos의 미니맵을 갱신한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateMiniMap(Vector2Int pPos)
    {
        miniMap.UpdateMiniMapPos(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 미니맵 텍스처를 받아온다.
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
}
