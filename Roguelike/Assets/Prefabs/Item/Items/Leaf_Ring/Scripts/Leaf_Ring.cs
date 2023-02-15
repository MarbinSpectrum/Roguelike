using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf_Ring : ItemObj
{
    public SpriteRenderer spriteRenderer;
    public SoundObj getSound;
    public override void GetItem()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        if (inventoryManager.AddItem(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);

            spriteRenderer.enabled = false;
            getSound.PlaySE();
        }
    }
}
