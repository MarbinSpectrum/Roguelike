using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : ���� ��ü���� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class MonsterManager : DontDestroySingleton<MonsterManager>
{
    private List<MonsterObj> fieldMonster = new List<MonsterObj>();     //�ʵ��� ���� ���
    private HashSet<Vector2Int> moveToPos = new HashSet<Vector2Int>();  //�̹� �̵��ϱ�� ������ ��ġ
    private Dictionary<Obj, MonsterObj> monsterObjs;                    
    private Dictionary<Obj, Sprite> previewSprites;
    private Dictionary<Obj, MonsterData> monsterDatas;
    private Dictionary<Obj, Queue<MonsterObj>> monsterQueue
        = new Dictionary<Obj, Queue<MonsterObj>>();

    [SerializeField]
    private List<MonsterData> monsterDataList = new List<MonsterData>();
    [SerializeField]
    private int monsterActRange = 10;

    private bool initData = false;
    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ������ �ʱ�ȭ�Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void Init()
    {
        if (initData)
            return;
        initData = true;

        if (previewSprites == null)
            previewSprites = new Dictionary<Obj, Sprite>();
        if (monsterObjs == null)
            monsterObjs = new Dictionary<Obj, MonsterObj>();
        if (monsterDatas == null)
            monsterDatas = new Dictionary<Obj, MonsterData>();

        monsterObjs.Clear();
        previewSprites.Clear();
        monsterDatas.Clear();

        foreach (MonsterData monsterData in monsterDataList)
        {
            Obj monsterType = monsterData.monsterType;
            MonsterObj monsterObj = monsterData.monsterObj;
            monsterObj.Init(monsterData);

            monsterObjs[monsterType] = monsterObj;
            previewSprites[monsterType] = monsterData.previewSprite;
            monsterDatas[monsterType] = monsterData;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش���ġ�� �̵��Ѵٰ� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMoveToPos(int pX, int pY)
    {
        moveToPos.Add(new Vector2Int(pX, pY));
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش���ġ�� �̵��ϱ���� ��ġ���� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsMoveToPos(int pX, int pY)
    {
        if (moveToPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش� ��ġ�� ���Ͱ� �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public MonsterObj IsMonster(int pX,int pY)
    {
        foreach(MonsterObj monsterObj in fieldMonster)
            if (monsterObj.alive)
                if (monsterObj.pos.x == pX && monsterObj.pos.y == pY)
                    return monsterObj;
        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾�� ���� ����� ���͸� ã�´�.
    ////////////////////////////////////////////////////////////////////////////////
    public MonsterObj GetClosestMonster()
    {
        CharacterManager characterManager = CharacterManager.instance;
        Vector2 cPos = characterManager.CharactorTransPos();
        MonsterObj closestMonster = null;

        float dis = float.MaxValue;
        foreach (MonsterObj monsterObj in fieldMonster)
            if (monsterObj.alive)
            {
                Vector2 mPos = monsterObj.transform.position;
                float newDis = Vector2.Distance(cPos, mPos);
                if (newDis < dis)
                {
                    closestMonster = monsterObj;
                    dis = newDis;
                }

            }
        return closestMonster;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pMonsterType�� �˸´� ���� ��ü ����
    ////////////////////////////////////////////////////////////////////////////////
    public MonsterObj CreateMonsterObj(Obj pMonsterType)
    {
        if (monsterQueue.ContainsKey(pMonsterType) == false)
            monsterQueue[pMonsterType] = new Queue<MonsterObj>();

        MonsterObj monsterObj = null;
        if (monsterQueue[pMonsterType].Count > 0)
            monsterObj = monsterQueue[pMonsterType].Dequeue();

        if (monsterObj == null)
            monsterObj = Instantiate(monsterObjs[pMonsterType],transform);
        monsterObj.gameObject.SetActive(true);

        MonsterData monsterData = monsterDatas[monsterObj.monster];
        monsterObj.Init(monsterData);

        return monsterObj;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ��ü�� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveMonsterObj(MonsterObj pMonsterObj)
    {
        if (pMonsterObj == null)
            return;
        monsterQueue[pMonsterObj.monster].Enqueue(pMonsterObj);
        pMonsterObj.gameObject.SetActive(false);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ��� ���� ��ü�� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    private void RemoveAll_MonsterObj()
    {
        foreach (MonsterObj monsterObj in fieldMonster)
        {
            RemoveMonsterObj(monsterObj);
        }
        fieldMonster.Clear();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���͸� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateMonster(List<MapMonster> pMapMonsters)
    {
        Init();

        RemoveAll_MonsterObj();
        foreach (MapMonster mapMonster in pMapMonsters)
        {
            //���Ͱ� ������ ������ ����
            //���� ��ü�� �������ش�.
            Vector2Int pos = mapMonster.pos;
            MonsterObj monsterObj = CreateMonsterObj(mapMonster.monsterType);
            monsterObj.transform.parent = transform;
            monsterObj.SetPos(pos);

            //���� ��ü ���
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
        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;

        //�̵��� ������ �켱������ ���ؼ� ��ĭ������ ����� ó���ϱ� ���� �迭
        int[,] moveCost = new int[mapManager.arrayW, mapManager.arrayH];

        for (int x = 0; x < mapManager.arrayW; x++)
            for (int y = 0; y < mapManager.arrayH; y++)
                if (mapManager.IsWall(x, y))
                    moveCost[x, y] = 1000;      //���� ������ 1000
                else if (chestManager.IsChest(x, y))
                    moveCost[x, y] = 1000;      //������ ������ 1000
                else if(jarManager.IsJar(x,y))
                    moveCost[x, y] = 20;        //�׾Ƹ��� �ִ� ������ 20
                else
                    moveCost[x, y] = 10;        //������� 10
        moveCost[pFrom.x, pFrom.y] = 10;

        foreach(MonsterObj monsterObj in fieldMonster)
        {
            if (monsterObj.alive == false)
            {
                //������� ������ ��ġ�� Ȯ������ �ʴ´�.
                continue;
            }
            moveCost[monsterObj.pos.x, monsterObj.pos.y] = 100; //���Ͱ� �̹������ϴ� ���� 100
        }
        List<Vector2Int> route = Monster_AStar.AstartRoute(pFrom, pTo, moveCost);

        return route;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���Ϳ� ������ ��θ� ������ �ִ� ����ü
    ////////////////////////////////////////////////////////////////////////////////
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �ൿ�� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator RunMonster()
    {
        moveToPos.Clear();

        CharacterManager characterManager = CharacterManager.instance;
        Vector2Int characterPos = characterManager.CharactorGamePos();

        List<MonsterRoute> monsterRouteList = new List<MonsterRoute>();

        List<MonsterObj> runMonsters = new List<MonsterObj>();

        foreach (MonsterObj monsterObj in fieldMonster)
        {
            if(Vector2.Distance(characterPos,monsterObj.pos) > monsterActRange)
            {
                //���� Ȱ��ȭ ���� ���̴�.
                continue;
            }

            runMonsters.Add(monsterObj);
        }

        foreach (MonsterObj monsterObj in runMonsters)
        {
            //�ʵ��� ���Ϳ� �÷��̾��� �ִ� ��θ� ���Ѵ�.
            List<Vector2Int> route = AstartRoute(monsterObj.pos, characterPos);
            monsterRouteList.Add(new MonsterRoute(monsterObj, route));
        }

        monsterRouteList.Sort(delegate (MonsterRoute A, MonsterRoute B)
        {
            //�Ÿ��� ����� ������ �����Ѵ�.
            int ASize = A.route != null ? A.route.Count : int.MaxValue;
            int BSize = B.route != null ? B.route.Count : int.MaxValue;
            return ASize.CompareTo(BSize);
        });

        foreach (MonsterRoute monsterRoute in monsterRouteList)
        {
            //�Ÿ������� �����߱⿡
            //����� ������ ������ �ൿ�� ó���Ѵ�.
            List<Vector2Int> route = monsterRoute.route;
            MonsterObj monsterObj = monsterRoute.monsterObj;
            monsterObj.RunMonster(route);         
        }

        yield break;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : �̸����� �̹����� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Sprite GetSprite(Obj pObj)
    {
        Init();
        Sprite previewSprite = previewSprites[pObj];

        return previewSprite;
    }
}
