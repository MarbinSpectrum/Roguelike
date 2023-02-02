using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CoinItem : ItemObj
{
    public GameObject spawnAni;
    public GameObject getAni;
    public override void Init()
    {
        base.Init();

        spawnAni.SetActive(true);
    }

    public override void GetItem()
    {
        TotalUI totalUI = TotalUI.instance;
        if (totalUI.ItemSendToInventory(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);

            GetGoldEffect getGoldEffect = GetGoldEffect.instance;
            getGoldEffect.GetGoldEffectRun(itemObjData.count);

            spawnAni.SetActive(false);
            getAni.SetActive(true);
        }
    }
}
