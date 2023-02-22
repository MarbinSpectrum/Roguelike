using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using MyLib;

public class PlayData
{
    //����Ǿ��� ����
    //1.�÷��̾� ����
    //2.�κ��丮 ������
    //3.���� ��������

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

    public string stageName;

    public PlayData(InventoryManager pInv, CharacterManager pCha)
    {
        if (pInv == null || pCha == null)
        {
            return;
        }
        level = pCha.nowLevel;
        baseMaxHp = pCha.baseMaxHp;
        nowHp = pCha.nowHp;
        maxExp = pCha.maxExp;
        nowExp = pCha.nowExp;
        basePow = pCha.basePow;
        baseBalance = pCha.baseBalance;
        baseCriPer = pCha.baseCriPer;
        baseCriDamage = pCha.baseCriDamage;

        List<ItemObjData> etcitemList = pInv.GetItemList(ItemType.Etc);
        foreach (ItemObjData itemObjData in etcitemList)
        {
            etcItem.Add(new ItemObjData(itemObjData));
        }
        List<ItemObjData> weaponitemList = pInv.GetItemList(ItemType.Weapon);
        foreach (ItemObjData itemObjData in weaponitemList)
        {
            weaponItem.Add(new ItemObjData(itemObjData));
        }
        List<ItemObjData> accessaritemList = pInv.GetItemList(ItemType.Accessary);
        foreach (ItemObjData itemObjData in accessaritemList)
        {
            accessaryItem.Add(new ItemObjData(itemObjData));
        }

        stageName = "STAGE1-1";
    }

    public PlayData()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        ItemManager itemManager = ItemManager.instance;
        MapManager mapManager = MapManager.instance;

        if (inventoryManager == null || characterManager == null
            || itemManager == null || mapManager == null)
        { 
            return; 
        }
        level = 1;
        baseMaxHp = characterManager.startHp;
        nowHp = characterManager.startHp;
        maxExp = 10;
        nowExp = 0;
        basePow = characterManager.startPow;
        baseBalance = characterManager.startBalance;
        baseCriPer = characterManager.startCriRate;
        baseCriDamage = characterManager.startCriDamage;

        //���� ��� ����
        ItemObjData weaponObjData = itemManager.CreateItemObjData(characterManager.startWeapon);
        if (weaponObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            weaponObjData.equip = true;
            weaponItem.Add(weaponObjData);
        }

        //�Ǽ��縮 ��� ����
        ItemObjData accessaryObjData = itemManager.CreateItemObjData(characterManager.startAccessary);
        if (accessaryObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            accessaryObjData.equip = true;
            accessaryItem.Add(accessaryObjData);
        }

        stageName = mapManager.mapNameKey;
    }
}
