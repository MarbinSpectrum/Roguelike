using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 함정을 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class TrapManager : DontDestroySingleton<TrapManager>
{
    private Dictionary<Vector2Int, NeedleTrap> needleTrap = new Dictionary<Vector2Int, NeedleTrap>();
    private Queue<NeedleTrap> needleQueue = new Queue<NeedleTrap>();
    [SerializeField] private NeedleTrap needleTrapPrefab;



    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY 위치에 가시 함정을 설치한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateNeedleTrap(int pX, int pY) => CreateNeedleTrap(new Vector2Int(pX, pY));
    public void CreateNeedleTrap(Vector2Int pPos)
    {
        NeedleTrap trap = null;
        if (needleQueue.Count > 0)
        {
            //큐에서 아이템을 재활용
            trap = needleQueue.Dequeue();
        }
        else
        {
            //재활용하지 못했으면 아이템을 생성함
            trap = Instantiate(needleTrapPrefab, transform);
        }
        needleTrap[pPos] = trap;
        trap.gameObject.SetActive(true);
        trap.transform.position = new Vector3(CreateMap.tileSize * pPos.x, CreateMap.tileSize * pPos.y, 0);
        trap.Init(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 필드의 트랩 자동 작동
    ////////////////////////////////////////////////////////////////////////////////
    public void RunTrap()
    {
        foreach(KeyValuePair<Vector2Int,NeedleTrap> pair in needleTrap)
        {
            NeedleTrap needleTrap = pair.Value;
            needleTrap.Run();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 필드의 트랩 모두 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveAll_Trap()
    {
        List<Vector2Int> posList = new List<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, NeedleTrap> pair in needleTrap)
        {
            posList.Add(pair.Key);
        }
        foreach(Vector2Int pos in posList)
        {
            NeedleTrap trap = needleTrap[pos];
            trap.gameObject.SetActive(false);
            needleQueue.Enqueue(trap);
            needleTrap.Remove(pos);
        }

    }
}
