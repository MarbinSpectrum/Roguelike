using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Magazine : ItemObj
{
    public SpriteRenderer spriteRenderer;
    public SoundObj getSound;
    public override void GetItem()
    {
        CharacterManager characterManager = CharacterManager.instance;
        ItemManager itemManager = ItemManager.instance;

        characterManager.GetBullet(itemObjData.count);
        itemManager.RemoveItem(pos.x, pos.y);

        spriteRenderer.enabled = false;
        getSound.PlaySE();
    }
}
