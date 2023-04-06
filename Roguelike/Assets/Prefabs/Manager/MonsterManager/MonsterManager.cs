using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 몬스터 객체들을 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class MonsterManager : DontDestroySingleton<MonsterManager>
{
    private List<MonsterObj> fieldMonster = new List<MonsterObj>();     //필드의 몬스터 목록
    private HashSet<Vector2Int> moveToPos = new HashSet<Vector2Int>();  //이미 이동하기로 예정된 위치
    private Dictionary<Obj, MonsterObj> monsterObjs;                    
    private Dictionary<Obj, Sprite> previewSprites;
    private Dictionary<Obj, MonsterData> monsterDatas;
    private Dictionary<Obj, Queue<MonsterObj>> monsterQueue
        = new Dictionary<Obj, Queue<MonsterObj>>();

    [SerializeField]
    private List<MonsterData> monsterDataList = new List<MonsterData>();
    [SerializeField]
    private int monsterActRange = 10;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터 정보를 초기화한다.
    ////////////////////////////////////////////////////////////////////////////////
    private void Init()
    {
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
    /// : 해당위치로 이동한다고 등록한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMoveToPos(int pX, int pY)
    {
        moveToPos.Add(new Vector2Int(pX, pY));
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 해당위치가 이동하기로한 위치인지 확인한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsMoveToPos(int pX, int pY)
    {
        if (moveToPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 해당 위치에 몬스터가 있는지 확인한다.
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
    /// : 플레이어와 가장 가까운 몬스터를 찾는다.
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
    /// : pMonsterType에 알맞는 몬스터 객체 생성
    ////////////////////////////////////////////////////////////////////////////////
    public MonsterObj CreateMonsterObj(Obj pMonsterType)
    {
        if (monsterQueue.ContainsKey(pMonsterType) == false)
            monsterQueue[pMonsterType] = new Queue<MonsterObj>();

        MonsterObj monsterObj = null;
        if (monsterQueue[pMonsterType].Count > 0)
            monsterObj = monsterQueue[pMonsterType].Dequeue();

        if (monsterObj == null)
            monsterObj = Instantiate(monsterObjs[pMonsterType]);

        monsterObj.gameObject.SetActive(true);
        MonsterData monsterData = monsterDatas[monsterObj.monster];
        monsterObj.Init(monsterData);

        return monsterObj;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터 객체를 제거한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveMonsterObj(MonsterObj pMonsterObj)
    {
        if (pMonsterObj == null)
            return;
        monsterQueue[pMonsterObj.monster].Enqueue(pMonsterObj);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 모든 몬스터 객체를 제거한다.
    ////////////////////////////////////////////////////////////////////////////////
    private void RemoveAll_MonsterObj()
    {
        foreach (MonsterObj monsterObj in fieldMonster)
        {
            RemoveMonsterObj(monsterObj);
            monsterObj.gameObject.SetActive(false);
        }
        fieldMonster.Clear();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터를 생성한다.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateMonster(List<MapMonster> pMapMonsters)
    {
        Init();

        RemoveAll_MonsterObj();
        foreach (MapMonster mapMonster in pMapMonsters)
        {
            //몬스터가 생성될 정보를 토대로
            //몬스터 객체를 생성해준다.
            Vector2Int pos = mapMonster.pos;
            MonsterObj monsterObj = CreateMonsterObj(mapMonster.monsterType);
            monsterObj.transform.parent = transform;
            monsterObj.SetPos(pos);

            //몬스터 객체 등록
            fieldMonster.Add(monsterObj);

            yield return new WaitForSeconds(0.001f);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pFrom부터 pTo로 이동하는 경로를 구한다.
    ////////////////////////////////////////////////////////////////////////////////
    public List<Vector2Int> AstartRoute(Vector2Int pFrom, Vector2Int pTo)
    {
        MapManager mapManager = MapManager.instance;
        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;

        //이동할 방향의 우선순위를 위해서 각칸마다의 비용을 처리하기 위한 배열
        int[,] moveCost = new int[mapManager.arrayW, mapManager.arrayH];

        for (int x = 0; x < mapManager.arrayW; x++)
            for (int y = 0; y < mapManager.arrayH; y++)
                if (mapManager.IsWall(x, y))
                    moveCost[x, y] = 1000;      //벽의 순위는 1000
                else if (chestManager.IsChest(x, y))
                    moveCost[x, y] = 1000;      //상자의 순위는 1000
                else if(jarManager.IsJar(x,y))
                    moveCost[x, y] = 20;        //항아리가 있는 공간은 20
                else
                    moveCost[x, y] = 10;        //빈공간은 10
        moveCost[pFrom.x, pFrom.y] = 10;

        foreach(MonsterObj monsterObj in fieldMonster)
        {
            if (monsterObj.alive == false)
            {
                //살아있지 않으면 위치를 확인하지 않는다.
                continue;
            }
            moveCost[monsterObj.pos.x, monsterObj.pos.y] = 100; //몬스터가 이미존재하는 곳은 100
        }
        List<Vector2Int> route = Monster_AStar.AstartRoute(pFrom, pTo, moveCost);

        return route;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터와 몬스터의 경로를 가지고 있는 구조체
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
    /// : 몬스터의 행동을 실행한다.
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
                //몬스터 활성화 범위 밖이다.
                continue;
            }

            runMonsters.Add(monsterObj);
        }

        foreach (MonsterObj monsterObj in runMonsters)
        {
            //필드의 몬스터와 플레이어의 최단 경로를 구한다.
            List<Vector2Int> route = AstartRoute(monsterObj.pos, characterPos);
            monsterRouteList.Add(new MonsterRoute(monsterObj, route));
        }

        monsterRouteList.Sort(delegate (MonsterRoute A, MonsterRoute B)
        {
            //거리와 가까운 순으로 정렬한다.
            int ASize = A.route != null ? A.route.Count : int.MaxValue;
            int BSize = B.route != null ? B.route.Count : int.MaxValue;
            return ASize.CompareTo(BSize);
        });

        foreach (MonsterRoute monsterRoute in monsterRouteList)
        {
            //거리순으로 정렬했기에
            //가까운 순으로 몬스터의 행동을 처리한다.
            List<Vector2Int> route = monsterRoute.route;
            MonsterObj monsterObj = monsterRoute.monsterObj;
            monsterObj.RunMonster(route);         
        }

        yield break;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 미리보기 이미지를 출력한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Sprite GetSprite(Obj pObj)
    {
        Init();
        Sprite previewSprite = previewSprites[pObj];

        return previewSprite;
    }
}
