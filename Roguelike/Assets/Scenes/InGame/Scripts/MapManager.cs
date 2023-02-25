using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

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

    public bool showName = true;
    public string mapNameKey;

    [SerializeField]
    private CreateMap createMap;
    [SerializeField]
    private MiniMap miniMap;

    [SerializeField]
    private int actSize;

    private TileObj[,] tiles;

    ////////////////////////////////////////////////////////////////////////////////
    /// : Ÿ�ϸ��� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateTileMap()
    {
        yield return createMap.runCreateGameMap();

        tiles = createMap.tileObjs;

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
        return createMap.gameEndPos;
    }

    ////////////////////////////////////////////////////////////////////////////////
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
        if (tiles[x, y].tileType == TileType.Wall)
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
            tiles[x, y].tileType == TileType.Wall)
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
}
