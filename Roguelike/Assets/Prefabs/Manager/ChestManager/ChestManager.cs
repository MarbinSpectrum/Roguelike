using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 상자 객체들의 정보를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class ChestManager : FieldObjectSingleton<ChestManager>
{
    private HashSet<Vector2Int> chestPos = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, Obj> randomChestPos = new Dictionary<Vector2Int, Obj>();
    private Dictionary<Vector2Int, Obj> makeChestPos = new Dictionary<Vector2Int, Obj>();
    private Dictionary<Vector2Int, Chest> chestObjs = new Dictionary<Vector2Int, Chest>();
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

    public Chest GetChestObj(Vector2Int pPos)
    {
        if (chestObjs.ContainsKey(pPos))
            return chestObjs[pPos];
        return null;
    }

    public bool IsChest(int pX, int pY)
    {
        if (chestPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

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

                Chest chestObj = Instantiate(chestPrefabs[chestType]);
                chestObj.pos = cPos;
                chestObj.transform.position =
                    new Vector3(cPos.x * CreateMap.tileSize, cPos.y * CreateMap.tileSize, 0);
                chestObj.MakeChestItem();

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

            Chest chestObj = Instantiate(chestPrefabs[chestType]);
            chestObj.pos = cPos;
            chestObj.transform.position =
                new Vector3(cPos.x * CreateMap.tileSize, cPos.y * CreateMap.tileSize, 0);
            chestObj.MakeChestItem();

            chestObjs[cPos] = chestObj;
            chestPos.Add(cPos);

            yield return new WaitForSeconds(0.001f);
        }
    }

    public void RemoveChestObj(Vector2Int pPos)
    {
        Chest chestObj = GetChestObj(pPos);
        chestPos.Remove(pPos);
        chestObjs.Remove(pPos);
    }
}
