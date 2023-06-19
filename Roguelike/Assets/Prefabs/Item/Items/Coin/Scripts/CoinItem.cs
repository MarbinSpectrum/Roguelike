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
        float addGoldRate = (100 + GetAddGoldRate()) / 100f;
        itemObjData.count = (int)(itemObjData.count * addGoldRate);

        if (inventoryMgr.AddItem(itemObjData))
        {
            itemMgr.RemoveItem(pos.x, pos.y);

            GetGoldEffect getGoldEffect = uIEffectMgr.getGoldEffect;
            getGoldEffect.GetGoldEffectRun(itemObjData.count);

            spawnAni.SetActive(false);
            getAni.SetActive(true);
        }
    }

    public int GetAddGoldRate()
    {
        int rate = 0;
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();

        rate += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.AddGold);
        rate += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.AddGold);
        return rate;
    }
}
