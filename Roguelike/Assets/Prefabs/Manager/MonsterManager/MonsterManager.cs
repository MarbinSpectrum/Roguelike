using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : ���� ��ü���� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class MonsterManager : FieldObjectSingleton<MonsterManager>
{
    private List<MonsterObj> fieldMonster = new List<MonsterObj>();
    private HashSet<Vector2Int> isMonseter = new HashSet<Vector2Int>();
    private Dictionary<Obj, MonsterObj> monsterObjs;
    private Dictionary<Obj, Sprite> previewSprites;
    public List<Vector2Int> moveToPos = new List<Vector2Int>();

    [SerializeField]
    private List<MonsterData> monsterDatas = new List<MonsterData>();

    private void Init()
    {
        if (previewSprites == null)
            previewSprites = new Dictionary<Obj, Sprite>();
        if (monsterObjs == null)
            monsterObjs = new Dictionary<Obj, MonsterObj>();
        foreach (MonsterData monsterData in monsterDatas)
        {
            Obj monsterType = monsterData.monsterType;
            MonsterObj monsterObj = monsterData.monsterObj;
            monsterObjs[monsterType] = monsterObj;
            previewSprites[monsterType] = monsterData.previewSprite;
        }
    }

    public bool IsMonster(int pX,int pY)
    {
        if (isMonseter.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }


    public IEnumerator runCreateMonster(List<MapMonster> pMapMonsters)
    {
        Init();
        foreach(MapMonster mapMonster in pMapMonsters)
        {
            Vector2Int pos = mapMonster.pos;
            if (isMonseter.Contains(pos))
                continue;
            isMonseter.Add(pos);

            MonsterObj monsterObj = Instantiate(monsterObjs[mapMonster.monsterType]);
            monsterObj.pos = pos;
            monsterObj.transform.position = new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);

            fieldMonster.Add(monsterObj);

            yield return new WaitForSeconds(0.001f);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pFrom���� pTo�� �̵��ϴ� ��θ� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public List<Vector2Int> AstartRoute(Vector2Int pFrom, Vector2Int pTo)
    {
        MapManager mapManager = MapManager.instance;

        //�������� Ÿ���� üũ�Ѵ�.
        bool[,] isWall = new bool[mapManager.arrayW, mapManager.arrayH];

        for (int x = 0; x < mapManager.arrayW; x++)
            for (int y = 0; y < mapManager.arrayH; y++)
                if (mapManager.IsWall(x, y))
                    isWall[x, y] = true;
        isWall[pFrom.x, pFrom.y] = false;

        List<Vector2Int> route = Algorithm.AstartRoute(pFrom, pTo, isWall);
        return route;
    }

    private struct MonsterRoute
    {
        public MonsterObj monsterObj;
        public List<Vector2Int> route;
        public MonsterRoute(MonsterObj pMonsterObj, List<Vector2Int> pRoute)
        {
            monsterObj = pMonsterObj;
            route = pRoute;
        }
    }

    public IEnumerator RunMonster()
    {
        moveToPos.Clear();

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int characterPos = characterManager.character.GetPos();

        List<MonsterRoute> monsterRouteList = new List<MonsterRoute>();

        foreach (MonsterObj monsterObj in fieldMonster)
        {
            List<Vector2Int> route = AstartRoute(monsterObj.pos, characterPos);
            monsterRouteList.Add(new MonsterRoute(monsterObj, route));
        }

        monsterRouteList.Sort(delegate (MonsterRoute A, MonsterRoute B)
        {
            int ASize = A.route != null ? A.route.Count : int.MaxValue;
            int BSize = B.route != null ? B.route.Count : int.MaxValue;
            return ASize.CompareTo(BSize);
        });

        foreach (MonsterRoute monsterRoute in monsterRouteList)
        {
            List<Vector2Int> route = monsterRoute.route;
            if (route == null || route.Count == 0)
                continue;

            MonsterObj monsterObj = monsterRoute.monsterObj;

            if (monsterObj.sleep)
            {
                if (monsterObj.range > route.Count)
                    monsterObj.sleep = false;
                else
                    continue;
            }

            Vector2Int gamePos = route[route.Count - 1];

            if (isMonseter.Contains(gamePos))
            {
                //�ش���ġ�� ���Ͱ� �ִ�.
                continue;
            }
            if (moveToPos.Contains(gamePos))
            {
                //�ش���ġ�� ���Ͱ� �̵��ϱ�� �ߴ�.
                continue;
            }
            if (gamePos == characterPos)
            {
                //ĳ������ ���ݹ����� �ִ�.
                continue;
            }


            //�̵���ġ ����
            isMonseter.Remove(monsterObj.pos);
            isMonseter.Add(gamePos);
            moveToPos.Add(gamePos);
            monsterObj.pos = gamePos;

            //�������� �̵����
            Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
            StartCoroutine(MyLib.Action2D.MoveTo(monsterObj.transform, toPos, 0.2f));
        }

        yield break;
    }

    public Sprite GetSprite(Obj pObj)
    {
        Init();
        Sprite previewSprite = previewSprites[pObj];

        return previewSprite;
    }
}
