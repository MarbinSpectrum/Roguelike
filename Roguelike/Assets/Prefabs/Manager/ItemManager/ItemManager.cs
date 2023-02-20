using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : �������� ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////

public class ItemManager : FieldObjectSingleton<ItemManager>
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
    /// : pX pY ��ġ�� �������� �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsItem(int pX,int pY)
    {
        ItemObj itemObj = GetItem(pX, pY);
        if (itemObj == null)
            return false;
        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �޾ƿ´�.
    ////////////////////////////////////////////////////////////////////////////////
    public ItemObj GetItem(int pX,int pY)
    {
        ItemObj itemObj = null;
        if (items.ContainsKey(new Vector2Int(pX, pY)))
            itemObj = items[new Vector2Int(pX, pY)];
        return itemObj;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �����Ѵ�.
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
        ItemObjData itemObjData = null;
        if (itemDatas.ContainsKey(pItem))
            itemObjData = itemDatas[pItem].createItemObjData();
        return itemObjData;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �����Ѵ�.
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
    /// : pItemObjData�� ������ ���� ������ ���ȿ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public static void RunUseItemEffect(ItemObjData pItemObjData)
    {
        CharacterManager characterManager = CharacterManager.instance;
        TotalUI totalUI = TotalUI.instance;

        List<ItemStatData> useEffect = pItemObjData.itemStats;
        foreach (ItemStatData itemStatData in useEffect)
        {
            ItemStat itemStat = itemStatData.itemStat;
            int value = itemStatData.GetValue();
            switch (itemStat)
            {
                case ItemStat.Heal:
                    {
                        //�÷��̾� ȸ��ȿ��
                        characterManager.AddNowHp(value);
                    }
                    break;
            }
        }
        totalUI.UpdateHp();
        totalUI.UpdateShield();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ItemType�� �޴´�.
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
            case Item.Wood_Ring:
                return ItemType.Accessary;
            case Item.Coin:
            default:
                return ItemType.Etc;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ְ� ������ ������ ������
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
        }

        return pItem;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �ִ� ������ �˻�
    ////////////////////////////////////////////////////////////////////////////////
    public static bool CantTakeOff(Item pItem)
    {
        switch (pItem)
        {
            case Item.Curse_Life_Ring:
            case Item.Curse_Leaf_Ring:
            case Item.Curse_Angry_Ring:
            case Item.Curse_Coolness_Ring:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������������ �˻�
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
    /// : �����°��� �˻�
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
    /// : �������� �˻�
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
    /// : �����ۿ��� Ư�����ݰ��� ������ ��ȯ�ϴ� �Լ�
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
                //�ش� ���ݰ��� �����ش�.
                sum += itemStat.GetValue();
            }
        }
        return sum;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����ۿ��� Ư�����ݰ��� ��ȯ�ϴ� �Լ�
    ////////////////////////////////////////////////////////////////////////////////
    public static void SetTotalStatValue(ItemObjData pItemObjData, ItemStat pItemStat, int pValue)
    {
        if (pItemObjData == null)
            return;

        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //�ش� ���ݰ��� 0���� �ʱ�ȭ�Ѵ�.
                itemStat.SetValue(0);
            }
        }

        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //���ݰ��� �����ش�.
                itemStat.SetValue(pValue);
                return;
            }
        }

        //�ش罺���� �׳� ���°� ����.
        //���� �߰����ش�.
        ItemStatData itemStatData = new ItemStatData(pItemStat, pValue, pValue);
        itemStatData.GetValue();
        pItemObjData.itemStats.Add(itemStatData);
    }
}
