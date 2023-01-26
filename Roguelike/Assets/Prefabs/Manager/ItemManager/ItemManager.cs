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

    private void Init()
    {
        if (itemObjs == null)
        {
            itemObjs = new Dictionary<Item, ItemObj>();
            foreach (ItemData itemData in itemDataList)
            {
                itemData.itemObj.itemObjData.itemData = itemData;
                itemObjs[itemData.item] = itemData.itemObj;
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
        itemObj.itemObjData = itemObjs[pItem].itemObjData;
        itemObj.pos = new Vector2Int(pX, pY);
        itemObj.transform.position =
            new Vector3(CreateMap.tileSize * pX, CreateMap.tileSize * pY, 0);
        itemObj.Init();

        items[new Vector2Int(pX, pY)] = itemObj;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveItem(int pX, int pY)
    {
        Init();

        if (IsItem(pX, pY))
        {
            ItemObj itemObj = items[new Vector2Int(pX, pY)];
            itemObj.gameObject.SetActive(false);
            items.Remove(new Vector2Int(pX, pY));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ItemType�� �޴´�.
    ////////////////////////////////////////////////////////////////////////////////
    public ItemType GetItemType(ItemObj pItemObj)
    {
        ItemObjData itemObjData = pItemObj.itemObjData;
        ItemData itemData = itemObjData.itemData;
        return GetItemType(itemData.item);
    }
    public ItemType GetItemType(Item pItem)
    {
        switch(pItem)
        {
            default:
                return ItemType.Etc;
        }
    }
}
