using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 아이템의 정보를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////

public class ItemManager : DontDestroySingleton<ItemManager>
{
    [SerializeField]
    private List<ItemData> itemDataList = new List<ItemData>();
    private Dictionary<Vector2Int, ItemObj> items = new Dictionary<Vector2Int, ItemObj>();
    private Dictionary<Item, ItemObj> itemObjs;
    private Dictionary<Item, ItemData> itemDatas;

    private void Init()
    {
        if (itemObjs == null)
        {
            itemObjs = new Dictionary<Item, ItemObj>();
            itemDatas = new Dictionary<Item, ItemData>();
            foreach (ItemData itemData in itemDataList)
            {
                itemObjs[itemData.item] = itemData.itemObj;
                itemDatas[itemData.item] = itemData;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY 위치에 아이템이 있는지 확인한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsItem(int pX,int pY)
    {
        ItemObj itemObj = GetItem(pX, pY);
        if (itemObj == null)
            return false;
        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY 위치에 아이템을 받아온다.
    ////////////////////////////////////////////////////////////////////////////////
    public ItemObj GetItem(int pX,int pY)
    {
        ItemObj itemObj = null;
        if (items.ContainsKey(new Vector2Int(pX, pY)))
            itemObj = items[new Vector2Int(pX, pY)];
        return itemObj;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY 위치에 아이템을 생성한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateItem(int pX, int pY,Item pItem)
    {
        Init();

        if (IsItem(pX, pY))
            return;
        if (itemObjs.ContainsKey(pItem) == false)
            return;
        if (itemObjs[pItem] == null)
            return;
        ItemObj itemObj = Instantiate(itemObjs[pItem]);
        itemObj.itemObjData = CreateItemObjData(pItem);
        itemObj.pos = new Vector2Int(pX, pY);
        itemObj.transform.position =
            new Vector3(CreateMap.tileSize * pX, CreateMap.tileSize * pY, 0);
        itemObj.Init();

        items[new Vector2Int(pX, pY)] = itemObj;
    }

    public ItemObjData CreateItemObjData(Item pItem)
    {
        Init();
        ItemData itemData = GetItemData(pItem);
        if(itemData != null)
        {
            ItemObjData itemObjData = itemData.createItemObjData();
            return itemObjData;
        }
        return null;
    }

    public ItemData GetItemData(Item pItem)
    {
        Init();
         if (itemDatas.ContainsKey(pItem))
            return itemDatas[pItem];
        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY 위치에 아이템을 제거한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveItem(int pX, int pY)
    {
        Init();

        if (IsItem(pX, pY))
        {
            items.Remove(new Vector2Int(pX, pY));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData의 정보를 토대로 아이템 사용효과를 적용
    ////////////////////////////////////////////////////////////////////////////////
    public static void RunUseItemEffect(ItemObjData pItemObjData)
    {
        CharacterManager characterManager = CharacterManager.instance;
        TotalUI totalUI = TotalUI.instance;

        List<ItemStatData> useEffect = pItemObjData.itemStats;
        foreach (ItemStatData itemStatData in useEffect)
        {
            ItemStat itemStat = itemStatData.itemStat;
            int value = itemStatData.dataValue;
            switch (itemStat)
            {
                case ItemStat.Heal:
                    {
                        //플레이어 회복효과
                        characterManager.AddNowHp(value);
                    }
                    break;
            }
        }
        totalUI.UpdateHp();
        totalUI.UpdateShield();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ItemType을 받는다.
    ////////////////////////////////////////////////////////////////////////////////
    public static ItemType GetItemType(ItemObj pItemObj)
    {
        ItemObjData itemObjData = pItemObj.itemObjData;
        return GetItemType(itemObjData);
    }
    public static ItemType GetItemType(ItemObjData pItemObjData)
    {
        ItemData itemData = pItemObjData.itemData;
        return GetItemType(itemData.item);
    }
    public static ItemType GetItemType(ItemData pItemData)
    {
        Item item = pItemData.item;
        return GetItemType(item);
    }
    public static ItemType GetItemType(Item pItem)
    {
        switch(pItem)
        {
            case Item.Glock17:
            case Item.M4:
            case Item.MP133:
            case Item.NormalGun:
                return ItemType.Weapon;
            case Item.Angry_Ring:
            case Item.Curse_Angry_Ring:

            case Item.Coolness_Ring:
            case Item.Curse_Coolness_Ring:

            case Item.Guardian_Ring:

            case Item.Life_Ring:
            case Item.Curse_Life_Ring:

            case Item.Gold_Ring:
            case Item.Silver_Ring:

            case Item.Leaf_Ring:
            case Item.Curse_Leaf_Ring:

            case Item.Skull_Ring:
            case Item.Curse_Skull_Ring:

            case Item.Wood_Ring:
                return ItemType.Accessary;
            case Item.Coin:
            default:
                return ItemType.Etc;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 저주가 해제된 상태의 아이템
    ////////////////////////////////////////////////////////////////////////////////
    public static Item UnCurseItem(Item pItem)
    {
        switch (pItem)
        {
            case Item.Curse_Leaf_Ring:
                return Item.Leaf_Ring;
            case Item.Curse_Life_Ring:
                return Item.Life_Ring;
            case Item.Curse_Angry_Ring:
                return Item.Angry_Ring;
            case Item.Curse_Coolness_Ring:
                return Item.Coolness_Ring;
            case Item.Curse_Skull_Ring:
                return Item.Skull_Ring;
        }

        return pItem;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 벗을수 있는 템인지 검사
    ////////////////////////////////////////////////////////////////////////////////
    public static bool CantTakeOff(Item pItem)
    {
        switch (pItem)
        {
            case Item.Curse_Life_Ring:
            case Item.Curse_Leaf_Ring:
            case Item.Curse_Angry_Ring:
            case Item.Curse_Coolness_Ring:
            case Item.Curse_Skull_Ring:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 사용아이템인지 검사
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsUseItem(Item pItem)
    {
        switch (pItem)
        {
            case Item.Potion:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 마쉬는건지 검사
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsDrink(Item pItem)
    {
        switch (pItem)
        {
            case Item.Potion:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 샷건인지 검사
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsShotGun(Item pItem)
    {
        switch (pItem)
        {
            case Item.MP133:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템에서 특정스텟값의 총합을 반환하는 함수
    ////////////////////////////////////////////////////////////////////////////////
    public static int GetTotalStatValue(List<ItemObjData> pItemList,ItemStat pItemStat)
    {
        if (pItemList == null)
            return 0;
        int sum = 0;
        foreach (ItemObjData itemObj in pItemList)
        {
            sum += GetTotalStatValue(itemObj, pItemStat);
        }
        return sum;
    }

    public static int GetTotalStatValue(ItemObjData pItemObjData, ItemStat pItemStat)
    {
        if (pItemObjData == null)
            return 0;
        int sum = 0;
        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //해당 스텟값을 더해준다.
                sum += itemStat.dataValue;
            }
        }
        return sum;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템에서 특정스텟값의 변환하는 함수
    ////////////////////////////////////////////////////////////////////////////////
    public static void SetTotalStatValue(ItemObjData pItemObjData, ItemStat pItemStat, int pValue)
    {
        if (pItemObjData == null)
            return;

        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //해당 스텟값을 0으로 초기화한다.
                itemStat.dataValue = 0;
            }
        }

        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //스텟값을 정해준다.
                itemStat.dataValue = pValue;
                return;
            }
        }

        //해당스텟이 그냥 없는것 같다.
        //새로 추가해준다.
        ItemStatData itemStatData = new ItemStatData(pItemStat, pValue, pValue);
        int dValue = itemStatData.dataValue;
        pItemObjData.itemStats.Add(itemStatData);
    }
}
