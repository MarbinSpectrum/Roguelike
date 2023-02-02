using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : ItemObj
{
    public override void GetItem()
    {
        TotalUI totalUI = TotalUI.instance;
        if (totalUI.ItemSendToInventory(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);

            gameObject.SetActive(false);
        }
    }
}
