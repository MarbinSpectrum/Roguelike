using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 아이템의 정보를 관리하는 매니저
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
        ItemObjData itemObjData = null;
        if (itemDatas.ContainsKey(pItem))
            itemObjData = itemDatas[pItem].createItemObjData();
        return itemObjData;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY 위치에 아이템을 제거한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveItem(int pX, int pY)
    {
        Init();

        if (IsItem(pX, pY))
        {
            ItemObj itemObj = items[new Vector2Int(pX, pY)];
            items.Remove(new Vector2Int(pX, pY));
        }
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
            case Item.Glasses_B:
            case Item.Glasses_R:
            case Item.Guardian_Ring:
            case Item.Life_Ring:
            case Item.Curse_Life_Ring:
            case Item.Gold_Ring:
                return ItemType.Accessary;
            case Item.Coin:
            default:
                return ItemType.Etc;
        }
    }

    public static bool CantTakeOff(Item pItem)
    {
        switch (pItem)
        {
            case Item.Curse_Life_Ring:
                return true;
            default:
                return false;
        }
        return false;
    }

    public static bool IsShotGun(Item pItem)
    {
        switch (pItem)
        {
            case Item.MP133:
                return true;
            default:
                return false;
        }
        return false;
    }
}
