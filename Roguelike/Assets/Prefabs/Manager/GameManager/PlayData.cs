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
    //3.현재 스테이지

    public uint level;

    public int baseMaxHp;
    public int nowHp;

    public uint maxExp;
    public uint nowExp;

    public int basePow;

    public int baseBalance;

    public float baseCriPer;
    public float baseCriDamage;

    public int maxBullet;
    public int nowBullet;

    public List<ItemObjData> etcItem = new List<ItemObjData>();
    public List<ItemObjData> weaponItem = new List<ItemObjData>();
    public List<ItemObjData> accessaryItem = new List<ItemObjData>();

    public string stageName;

    public PlayData(InventoryManager pInv, CharacterManager pCha)
    {
        MapManager mapManager = MapManager.instance;

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
        maxBullet = pCha.maxBullet;
        nowBullet = pCha.nowBullet;

        List<ItemObjData> etcitemList = pInv.GetItemList(ItemType.Etc);
        etcItem = etcitemList;
        List<ItemObjData> weaponitemList = pInv.GetItemList(ItemType.Weapon);
        weaponItem = weaponitemList;
        List<ItemObjData> accessaritemList = pInv.GetItemList(ItemType.Accessary);
        accessaryItem = accessaritemList;
        stageName = mapManager.mapNameKey;
    }

    public PlayData(bool pDefault)
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
        maxBullet = characterManager.startBullet;
        nowBullet = characterManager.startBullet;

        //무기 장비 설정
        ItemObjData weaponObjData = itemManager.CreateItemObjData(characterManager.startWeapon);
        if (weaponObjData != null)
        {
            //해당 장비를 장착한다.
            weaponObjData.equip = true;
            weaponItem.Add(weaponObjData);
        }

        //악세사리 장비 설정
        ItemObjData accessaryObjData = itemManager.CreateItemObjData(characterManager.startAccessary);
        if (accessaryObjData != null)
        {
            //해당 장비를 장착한다.
            accessaryObjData.equip = true;
            accessaryItem.Add(accessaryObjData);
        }

        stageName = mapManager.mapNameKey;
    }
}
