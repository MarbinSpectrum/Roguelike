using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

////////////////////////////////////////////////////////////////////////////////
/// : ��ü���� UI�� �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
public class TotalUI : FieldObjectSingleton<TotalUI>
{
    [SerializeField]
    private LoadingUI loadingUI;
    [SerializeField]
    private ShieldBar shieldBar;
    [SerializeField]
    private HpBar hpBar;
    [SerializeField]
    private ExpBar expBar;
    [SerializeField]
    private KeyPad keyPad;
    [SerializeField]
    private MapName mapName;
    [SerializeField]
    private MiniMapUI miniMap;
    [SerializeField]
    private SetingUI setingUI;
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private PlayerDataUI playerDataUI;
    [SerializeField]
    private ItemDataUI itemDataUI;
    [SerializeField]
    private SelectStatUI selectStatUI;
    [SerializeField]
    private GunBenchUI gunBenchUI;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �� ������������ ǥ�����ݴϴ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowCreateMap(bool pState)
    {
        loadingUI.ActLoading(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ü�� ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateHp(int pMaxHp,int pNowHp)
    {
        hpBar.UpdateHp(pMaxHp, pNowHp);
    }
    public void UpdateHp()
    {
        CharacterManager characterManager = CharacterManager.instance;

        int maxHp = characterManager.GetTotalMaxHp();
        int nowHp = characterManager.nowHp;
        UpdateHp(maxHp, nowHp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateShield(int pNowShield)
    {
        shieldBar.UpdateShield(pNowShield);
    }
    public void UpdateShield()
    {
        CharacterManager characterManager = CharacterManager.instance;

        int nowShield = characterManager.GetTotalShield();
        UpdateShield(nowShield);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ġ ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateExp(uint pMaxExp, uint pNowExp)
    {
        expBar.UpdateExp(pMaxExp, pNowExp);
    }
    public void UpdateExp()
    {
        CharacterManager characterManager = CharacterManager.instance;

        uint maxExp = characterManager.maxExp;
        uint nowExp = characterManager.nowExp;
        expBar.UpdateExp(maxExp, nowExp);
    }

    public void ShowMapName(string pStr,bool pPlayAni)
    {
        mapName.ShowMapName(pStr, pPlayAni);
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ݰ���UI ����
    ////////////////////////////////////////////////////////////////////////////////
    public void ActSelectStatUI(bool pState)
    {
        selectStatUI.ActSelectStat(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �κ��丮 ����
    ////////////////////////////////////////////////////////////////////////////////
    public void ActInventory(bool pState)
    {
        inventoryUI.ActInventory(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ѱ� �۾��� UI ����
    ////////////////////////////////////////////////////////////////////////////////
    public void ActGunBench(bool pState)
    {
        gunBenchUI.ActUI(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData(����������)�� ���� ������ ������ ǥ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowItemData(ItemObjData pItemObjData,int pIdx)
    {
        itemDataUI.ActItemData(true);
        itemDataUI.UpdateItemData(pItemObjData, pIdx);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemType�� �´� �κ��丮 �׸��� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateInventory(ItemType pItemType)
    {
        inventoryUI.UpdateSlot(pItemType);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Ű�е��� �۵��� �����.
    ////////////////////////////////////////////////////////////////////////////////
    public void ActKeyPad(bool pState)
    {
        keyPad.ActKeyPad(pState);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gunBenchUI.isRun)
            {
                gunBenchUI.ActUI();
            }
            else if (itemDataUI.isRun)
            {
                itemDataUI.ActItemData();
            }
            else if (inventoryUI.isRun)
            {
                inventoryUI.ActInventory();
            }
            else if (playerDataUI.isRun)
            {
                playerDataUI.ActPlayerData();
            }
            else
            {
                ActKeyPad(setingUI.gameObject.activeSelf);
                setingUI.gameObject.SetActive(!setingUI.gameObject.activeSelf);
            }
        }
    }
}
