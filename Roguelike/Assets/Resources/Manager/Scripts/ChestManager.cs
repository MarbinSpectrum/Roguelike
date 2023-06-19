using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 상자 객체들의 정보를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class ChestManager : DontDestroySingleton<ChestManager>
{
    private HashSet<Vector2Int> chestPos = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, Obj> randomChestPos = new Dictionary<Vector2Int, Obj>();
    private Dictionary<Vector2Int, Obj> makeChestPos = new Dictionary<Vector2Int, Obj>();
    private Dictionary<Vector2Int, Chest> chestObjs = new Dictionary<Vector2Int, Chest>();
    private Dictionary<Obj, Queue<Chest>> chestQueue = new Dictionary<Obj, Queue<Chest>>();

    [SerializeField, Range(0, 100)]
    private int chestRate;
    [SerializeField]
    private Dictionary<Obj, Chest> chestPrefabs = new Dictionary<Obj, Chest>();


    ////////////////////////////////////////////////////////////////////////////////
    /// : 무조건 깔리는 상자 위치를 등록
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMakeChestPos(Vector2Int pPos, Obj pChestType)
    {
        makeChestPos.Add(pPos, pChestType);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 임의로 깔리는 상자 위치를 등록
    ////////////////////////////////////////////////////////////////////////////////
    public void AddRandomChestPos(Vector2Int pPos,Obj pChestType)
    {
        randomChestPos.Add(pPos, pChestType);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos위치에 궤작이 있다면 궤작 객체를 가져온다.
    ////////////////////////////////////////////////////////////////////////////////
    public Chest GetChestObj(Vector2Int pPos)
    {
        if (chestObjs.ContainsKey(pPos))
            return chestObjs[pPos];
        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY위치에 궤작이 있는지 확인한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsChest(int pX, int pY)
    {
        if (chestPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pChestType에 알맞는 궤작 오브젝트 생성 및 풀에서 가져오기
    ////////////////////////////////////////////////////////////////////////////////
    public Chest CreateChestObj(Obj pChestType)
    {
        if(chestQueue.ContainsKey(pChestType) == false)
            chestQueue[pChestType] = new Queue<Chest>();

        Chest chest = null;
        if (chestQueue[pChestType].Count > 0)
            chest = chestQueue[pChestType].Dequeue();

        if (chest == null)
            chest = Instantiate(chestPrefabs[pChestType],transform);
        chest.gameObject.SetActive(true);
        chest.Init();

        return chest;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos에 해당하는 궤작을 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveChestObj(Vector2Int pPos)
    {
        Chest chestObj = GetChestObj(pPos);
        if (chestObj == null)
            return;
        chestQueue[chestObj.chestType].Enqueue(chestObj);
        chestPos.Remove(pPos);
        chestObjs.Remove(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 모든 궤작을 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveAll_ChestObj()
    {
        foreach (KeyValuePair<Obj, Queue<Chest>> chestQ in chestQueue)
        {
            Queue<Chest> tempQueue = new Queue<Chest>();
            while (chestQ.Value.Count > 0)
            {
                Chest chestObj = chestQ.Value.Dequeue();
                chestObj.gameObject.SetActive(false);
                chestObj.Init();
                tempQueue.Enqueue(chestObj);
            }
            while (tempQueue.Count > 0)
            {
                Chest chestObj = tempQueue.Dequeue();
                chestQ.Value.Enqueue(chestObj);
            }
        }

        foreach (Vector2Int cPos in chestPos)
        {
            Chest chestObj = GetChestObj(cPos);
            if (chestObj == null)
                continue;
            chestObj.gameObject.SetActive(false);
            chestQueue[chestObj.chestType].Enqueue(chestObj);
        }
        randomChestPos.Clear();
        makeChestPos.Clear();
        chestPos.Clear();
        chestObjs.Clear();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 궤작 객체를 지정된 곳에 생성
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateChestObj()
    {
        //임의로 깔리는 상자다.
        //어떤 상자를 설치할지 난수를 생성해서 구해준다.
        int maxNum = randomChestPos.Count;
        int spawnNum = (int)(maxNum * (chestRate / (float)100));
        List<int> numCnt = MyLib.Algorithm.CreateRandomList(maxNum, spawnNum);
        HashSet<int> numHash = new HashSet<int>();
        numCnt.ForEach(x => numHash.Add(x));

        //임의의 상자를 깔아준다.
        int idx = 0;
        foreach (KeyValuePair<Vector2Int, Obj> chestData in randomChestPos)
        {
            Obj chestType = chestData.Value;
            Vector2Int cPos = chestData.Key;
            if (numCnt.Contains(idx++))
            {
                Chest chestObj = CreateChestObj(chestType);
                chestObj.pos = cPos;
                chestObj.transform.position =
                    new Vector3(cPos.x * CreateMap.tileSize, cPos.y * CreateMap.tileSize, 0);
                chestObj.transform.parent = transform;

                chestObjs[cPos] = chestObj;
                chestPos.Add(cPos);

                yield return new WaitForSeconds(0.001f);
            }
            else
            {
                chestPos.Remove(cPos);
            }
        }

        //무조건 깔리는 상자위치
        foreach (KeyValuePair<Vector2Int, Obj> chestData in makeChestPos)
        {
            Obj chestType = chestData.Value;
            Vector2Int cPos = chestData.Key;

            Chest chestObj = CreateChestObj(chestType);
            chestObj.pos = cPos;
            chestObj.transform.position =
                new Vector3(cPos.x * CreateMap.tileSize, cPos.y * CreateMap.tileSize, 0);
            chestObj.transform.parent = transform;

            chestObjs[cPos] = chestObj;
            chestPos.Add(cPos);

            yield return new WaitForSeconds(0.001f);
        }
    }
}
