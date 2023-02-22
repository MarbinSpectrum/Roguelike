using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[SerializeField]
public class ItemObjData
{
    public int count;
    private ItemData ItemData;
    public ItemData itemData
    {
        get
        {
            if(ItemData == null)
            {
                ItemManager itemManager = ItemManager.instance;
                ItemData = itemManager.GetItemData(item);
            }
            return ItemData;
        }
        set
        {
            ItemData = value;
        }
    }
    public Item item;
    public List<ItemStatData> itemStats = new List<ItemStatData>();
    public bool equip;

    public ItemObjData()
    {
        count = 0;
        itemData = null;
        item = Item.Null;
        itemStats.Clear();
        equip = false;
    }

    public ItemObjData(ItemObjData pItemObjData)
    {
        count = pItemObjData.count;
        itemData = pItemObjData.itemData;
        item = itemData.item;
        itemStats.Clear();
        foreach (ItemStatData itemStatData in pItemObjData.itemStats)
            itemStats.Add(itemStatData);
        equip = pItemObjData.equip;
    }
}

public class ItemObj : SerializedMonoBehaviour
{
    [System.NonSerialized]
    public Vector2Int pos;
    [System.NonSerialized]
    public ItemObjData itemObjData;

    public virtual void Init()
    {
    }

    public virtual void GetItem()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        if (inventoryManager.AddItem(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);
        }
    }
}
