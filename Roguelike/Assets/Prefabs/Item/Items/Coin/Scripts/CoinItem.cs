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

    public int GetAddGoldRate()
    {
        int rate = 0;
        CharacterManager characterManager = CharacterManager.instance;
        List<ItemObjData> nowAccessary = characterManager.NowAccessaryList();
        ItemObjData nowWeapon = characterManager.NowWeapon();

        if (nowWeapon != null)
        {
            foreach (ItemStatData itemStatData in nowWeapon.itemStats)
            {
                if (itemStatData.itemStat == ItemStat.AddGold)
                {
                    //AddGoldΩ∫≈›¿Ã¥Ÿ ±›»≠ »πµÊ∑¸ø° ¥ı«ÿ¡ÿ¥Ÿ.
                    rate += itemStatData.GetValue();
                }
            }
        }

        foreach(ItemObjData accessary in nowAccessary)
        {
            foreach (ItemStatData itemStat in accessary.itemStats)
            {
                if (itemStat.itemStat == ItemStat.AddGold)
                {
                    //AddGoldΩ∫≈›¿Ã¥Ÿ ±›»≠ »πµÊ∑¸ø° ¥ı«ÿ¡ÿ¥Ÿ.
                    rate += itemStat.GetValue();
                }
            }
        }
        return rate;
    }
}
