using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemObjData
{
    public int count;
    public ItemData itemData;
    public List<ItemStatData> itemStats = new List<ItemStatData>();

    public ItemObjData()
    {
        count = 0;
        itemData = null;
        itemStats.Clear();
    }

    public ItemObjData(ItemObjData pItemObjData)
    {
        count = pItemObjData.count;
        itemData = pItemObjData.itemData;
        itemStats.Clear();
        foreach (ItemStatData itemStatData in pItemObjData.itemStats)
            itemStats.Add(itemStatData);
    }
}

public class ItemObj : SerializedMonoBehaviour
{
    [System.NonSerialized]
    public Vector2Int pos;
    [System.NonSerialized]
    public ItemObjData itemObjData = new ItemObjData();

    public virtual void Init()
    {
        if (itemObjData.itemData.stockItem)
        {
            itemObjData.count = Random.Range(
                itemObjData.itemData.valueMinMax.x,
                itemObjData.itemData.valueMinMax.y);
        }
        else
            itemObjData.count = 1;
    }

    public virtual void GetItem()
    {
        TotalUI totalUI = TotalUI.instance;
        if (totalUI.ItemSendToInventory(this))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);
        }
    }
}
