using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using MyLib;

public class PlayData
{
    //저장되야할 정보
    //1.플레이어 스텟
    //2.인벤토리 아이템

    public uint level;

    public int baseMaxHp;
    public int nowHp;

    public uint maxExp;
    public uint nowExp;

    public int basePow;

    public int baseBalance;

    public float baseCriPer;
    public float baseCriDamage;

    public List<ItemObjData> etcItem = new List<ItemObjData>();
    public List<ItemObjData> weaponItem = new List<ItemObjData>();
    public List<ItemObjData> accessaryItem = new List<ItemObjData>();

    public PlayData()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        if (inventoryManager == null || characterManager == null)
        { 
            return; 
        }
        level = characterManager.nowLevel;
        baseMaxHp = characterManager.baseMaxHp;
        nowHp = characterManager.nowHp;
        maxExp = characterManager.maxExp;
        nowExp = characterManager.nowExp;
        basePow = characterManager.basePow;
        baseBalance = characterManager.baseBalance;
        baseCriPer = characterManager.baseCriPer;
        baseCriDamage = characterManager.baseCriDamage;

        List<ItemObjData> etcitemList = inventoryManager.GetItemList(ItemType.Etc);
        foreach (ItemObjData item in etcitemList)
        {
            etcItem.Add(new ItemObjData(item));
        }
        List<ItemObjData> weaponitemList = inventoryManager.GetItemList(ItemType.Weapon);
        foreach (ItemObjData item in weaponitemList)
        {
            weaponItem.Add(new ItemObjData(item));
        }
        List<ItemObjData> accessaritemList = inventoryManager.GetItemList(ItemType.Accessary);
        foreach (ItemObjData item in accessaritemList)
        {
            accessaryItem.Add(new ItemObjData(item));
        }
    }
}
