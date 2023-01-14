using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : ��ü���� UI�� �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
public class TotalUI : FieldObjectSingleton<TotalUI>
{
    [SerializeField]
    private GameObject createMap;
    [SerializeField]
    private HpBar hpBar;
    [SerializeField]
    private ExpBar expBar;
    [SerializeField]
    private MiniMapUI miniMap;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �� ������������ ǥ�����ݴϴ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowCreateMap(bool pState)
    {
        createMap.SetActive(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ü�� ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateHp(uint pMaxHp,uint pNowHp)
    {
        hpBar.UpdateHp(pMaxHp, pNowHp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ġ ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateExp(uint pMaxExp, uint pNowExp)
    {
        expBar.UpdateExp(pMaxExp, pNowExp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos���� pDis�̳��� ������ �̴ϸʿ��� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateMiniMap(Vector2Int pPos, uint pDis)
    {
        //�����ٰ����� ť�� ���ؼ� Ž���Ѵ�.
        MapManager mapManager = MapManager.instance;
        Dictionary<Vector2Int,int> Map = new Dictionary<Vector2Int, int>();
        Queue<Vector2Int> Queue = new Queue<Vector2Int>();

        //8�������� Ž���Ѵ�.
        Vector2Int[] dic = new Vector2Int[8]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        Queue.Enqueue(pPos);
        mapManager.UpdateMiniMap(pPos);
        Map.Add(pPos,0);     
        while(Queue.Count > 0)
        {
            Vector2Int now = Queue.Dequeue();
            for(int i = 0; i < 8; i++)
            {
                Vector2Int next = new Vector2Int(now.x + dic[i].x, now.y + dic[i].y);
                if (Map.ContainsKey(next))
                {
                    //�̹�ǥ���� ����
                    continue;
                }
                if (Map[now] + 1 >= pDis)
                {
                    //���Ź��� �ʰ�
                    continue;
                }
                if (mapManager.IsWall(next.x, next.y) == true && 
                    mapManager.IsEdge(next.x, next.y) == false)
                {
                    //NULL�ΰ��� ǥ�þ���
                    continue;
                }
                if(mapManager.IsEdge(next.x, next.y) == false)
                {
                    //�����ڸ������� Ž������
                    Queue.Enqueue(next);
                }
                mapManager.UpdateMiniMap(next);
                Map.Add(next,Map[now]+1);
            }
        }

        //�ؽ��ĺ���
        Texture2D miniMapTexture = mapManager.GetMiniMapTexture();
        miniMap.UpdateMiniMap(miniMapTexture);
    }
}
