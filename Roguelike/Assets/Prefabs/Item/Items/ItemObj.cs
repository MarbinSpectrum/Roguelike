using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemObjData
{
    public int count;
    public ItemData itemData;
    public List<ItemStatData> itemStats = new List<ItemStatData>();
    public bool equip;

    public ItemObjData()
    {
        count = 0;
        itemData = null;
        itemStats.Clear();
        equip = false;
    }

    public ItemObjData(ItemObjData pItemObjData)
    {
        count = pItemObjData.count;
        itemData = pItemObjData.itemData;
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
        TotalUI totalUI = TotalUI.instance;
        if (totalUI.ItemSendToInventory(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);
        }
    }
}
