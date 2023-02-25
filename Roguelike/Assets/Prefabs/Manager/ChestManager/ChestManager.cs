using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ���� ��ü���� ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class ChestManager : FieldObjectSingleton<ChestManager>
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
    /// : ������ �򸮴� ���� ��ġ�� ���
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMakeChestPos(Vector2Int pPos, Obj pChestType)
    {
        makeChestPos.Add(pPos, pChestType);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���Ƿ� �򸮴� ���� ��ġ�� ���
    ////////////////////////////////////////////////////////////////////////////////
    public void AddRandomChestPos(Vector2Int pPos,Obj pChestType)
    {
        randomChestPos.Add(pPos, pChestType);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos��ġ�� ������ �ִٸ� ���� ��ü�� �����´�.
    ////////////////////////////////////////////////////////////////////////////////
    public Chest GetChestObj(Vector2Int pPos)
    {
        if (chestObjs.ContainsKey(pPos))
            return chestObjs[pPos];
        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY��ġ�� ������ �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsChest(int pX, int pY)
    {
        if (chestPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pChestType�� �˸´� ���� ������Ʈ ���� �� Ǯ���� ��������
    ////////////////////////////////////////////////////////////////////////////////
    public Chest CreateChestObj(Obj pChestType)
    {
        if(chestQueue.ContainsKey(pChestType) == false)
            chestQueue[pChestType] = new Queue<Chest>();

        Chest chest = null;
        if (chestQueue[pChestType].Count > 0)
            chest = chestQueue[pChestType].Dequeue();

        if (chest == null)
            chest = Instantiate(chestPrefabs[pChestType]);
        chest.gameObject.SetActive(true);
        chest.Init();

        return chest;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos�� �ش��ϴ� ������ ����
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
    /// : ��� ������ ����
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
    /// : ���� ��ü�� ������ ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateChestObj()
    {
        //���Ƿ� �򸮴� ���ڴ�.
        //� ���ڸ� ��ġ���� ������ �����ؼ� �����ش�.
        int maxNum = randomChestPos.Count;
        int spawnNum = (int)(maxNum * (chestRate / (float)100));
        List<int> numCnt = MyLib.Algorithm.CreateRandomList(maxNum, spawnNum);
        HashSet<int> numHash = new HashSet<int>();
        numCnt.ForEach(x => numHash.Add(x));

        //������ ���ڸ� ����ش�.
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

        //������ �򸮴� ������ġ
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
