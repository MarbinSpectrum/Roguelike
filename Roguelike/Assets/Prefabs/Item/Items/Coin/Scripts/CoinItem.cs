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

        InventoryManager inventoryManager = InventoryManager.instance;
        if (inventoryManager.AddItem(itemObjData))
        {
            ItemManager itemManager = ItemManager.instance;
            itemManager.RemoveItem(pos.x, pos.y);

            GetGoldEffect getGoldEffect = GetGoldEffect.instance;
            getGoldEffect.GetGoldEffectRun(itemObjData.count);

            spawnAni.SetActive(false);
            getAni.SetActive(true);
        }
    }

    public int GetAddGoldRate()
    {
        int rate = 0;
        InventoryManager inventoryManager = InventoryManager.instance;
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();
        ItemObjData nowWeapon = inventoryManager.NowWeapon();

        rate += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.AddGold);
        rate += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.AddGold);
        return rate;
    }
}
