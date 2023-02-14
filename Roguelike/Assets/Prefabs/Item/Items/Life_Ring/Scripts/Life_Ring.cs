using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_Ring : ItemObj
{
    public SpriteRenderer spriteRenderer;
    public SoundObj getSound;
    public override void GetItem()
    {
        TotalUI totalUI = TotalUI.instance;
        if (totalUI.ItemSendToInventory(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);

            spriteRenderer.enabled = false;
            getSound.PlaySE();
        }
    }
}
