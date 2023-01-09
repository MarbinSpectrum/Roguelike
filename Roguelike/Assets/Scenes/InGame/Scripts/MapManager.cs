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

    [SerializeField]
    private CreateMap createMap;

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
}
