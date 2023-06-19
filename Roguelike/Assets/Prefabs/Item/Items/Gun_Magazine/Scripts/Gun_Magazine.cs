using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Magazine : ItemObj
{
    public SpriteRenderer spriteRenderer;
    public SoundObj getSound;
    public override void GetItem()
    {
        characterMgr.GetBullet(itemObjData.count);
        itemMgr.RemoveItem(pos.x, pos.y);

        TotalUI totalUI = TotalUI.instance;

        totalUI.ShowExplainText(LanguageManager.GetText("AMMO_GET"));
        spriteRenderer.enabled = false;
        getSound.PlaySE();
    }
}
