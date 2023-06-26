using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class TrapManager : DontDestroySingleton<TrapManager>
{
    private Dictionary<Vector2Int, NeedleTrap> needleTrap = new Dictionary<Vector2Int, NeedleTrap>();
    private Queue<NeedleTrap> needleQueue = new Queue<NeedleTrap>();
    [SerializeField] private NeedleTrap needleTrapPrefab;



    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� ���� ������ ��ġ�Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateNeedleTrap(int pX, int pY) => CreateNeedleTrap(new Vector2Int(pX, pY));
    public void CreateNeedleTrap(Vector2Int pPos)
    {
        NeedleTrap trap = null;
        if (needleQueue.Count > 0)
        {
            //ť���� �������� ��Ȱ��
            trap = needleQueue.Dequeue();
        }
        else
        {
            //��Ȱ������ �������� �������� ������
            trap = Instantiate(needleTrapPrefab, transform);
        }
        needleTrap[pPos] = trap;
        trap.gameObject.SetActive(true);
        trap.transform.position = new Vector3(CreateMap.tileSize * pPos.x, CreateMap.tileSize * pPos.y, 0);
        trap.Init(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ʵ��� Ʈ�� �ڵ� �۵�
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
    /// : �ʵ��� Ʈ�� ��� ����
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
