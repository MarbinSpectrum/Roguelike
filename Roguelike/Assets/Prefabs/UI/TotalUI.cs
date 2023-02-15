using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

////////////////////////////////////////////////////////////////////////////////
/// : 전체적인 UI를 관리합니다.
////////////////////////////////////////////////////////////////////////////////
public class TotalUI : FieldObjectSingleton<TotalUI>
{
    [SerializeField]
    private LoadingUI loadingUI;
    [SerializeField]
    private HpBar hpBar;
    [SerializeField]
    private ExpBar expBar;
    [SerializeField]
    private KeyPad keyPad;
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : 맵 생성중인지를 표시해줍니다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowCreateMap(bool pState)
    {
        loadingUI.ActLoading(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 체력 상태 갱신
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
    /// : 경험치 상태 갱신
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos에서 pDis이내의 공간을 미니맵에서 갱신한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateMiniMap(Vector2Int pPos, uint pDis)
    {
        //밝혀줄공간을 큐를 통해서 탐색한다.
        MapManager mapManager = MapManager.instance;
        Dictionary<Vector2Int,int> Map = new Dictionary<Vector2Int, int>();
        Queue<Vector2Int> Queue = new Queue<Vector2Int>();

        //8방향으ㄹ 탐색한다.
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
                    //이미표시한 영역
                    continue;
                }
                if (Map[now] + 1 >= pDis)
                {
                    //갱신범위 초과
                    continue;
                }
                if (mapManager.IsWall(next.x, next.y) == true && 
                    mapManager.IsEdge(next.x, next.y) == false)
                {
                    //NULL인경우는 표시안함
                    continue;
                }
                if(mapManager.IsEdge(next.x, next.y) == false)
                {
                    //가장자리에서는 탐색중지
                    Queue.Enqueue(next);
                }
                mapManager.UpdateMiniMap(next);
                Map.Add(next,Map[now]+1);
            }
        }

        //텍스쳐변경
        Texture2D miniMapTexture = mapManager.GetMiniMapTexture();
        miniMap.UpdateMiniMap(miniMapTexture);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 스텟결정UI 실행
    ////////////////////////////////////////////////////////////////////////////////
    public void ActSelectStatUI(bool pState)
    {
        selectStatUI.ActSelectStat(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 인벤토리 실행
    ////////////////////////////////////////////////////////////////////////////////
    public void ActInventory(bool pState)
    {
        inventoryUI.ActInventory(pState);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData(아이템정보)를 토대로 아이템 정보를 표시한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowItemData(ItemObjData pItemObjData,int pIdx)
    {
        itemDataUI.ActItemData(true);
        itemDataUI.UpdateItemData(pItemObjData, pIdx);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemType에 맞는 인벤토리 항목을 보여준다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateInventory(ItemType pItemType)
    {
        inventoryUI.UpdateSlot(pItemType);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 키패드의 작동을 멈춘다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ActKeyPad(bool pState)
    {
        keyPad.ActKeyPad(pState);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (itemDataUI.isRun)
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
