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

    public int maxBullet;
    public int nowBullet;

    public List<ItemObjData> etcItem = new List<ItemObjData>();
    public List<ItemObjData> weaponItem = new List<ItemObjData>();
    public List<ItemObjData> accessaryItem = new List<ItemObjData>();

    public bool gunBenchAct;

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

        gunBenchAct = GameManager.gunBenchAct;

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
        MapManager mapManager = MapManager.instance;

        level = 1;
        baseMaxHp = Mgr.characterMgr.startHp;
        nowHp = Mgr.characterMgr.startHp;
        maxExp = 10;
        nowExp = 0;
        basePow = Mgr.characterMgr.startPow;
        baseBalance = Mgr.characterMgr.startBalance;
        baseCriPer = Mgr.characterMgr.startCriRate;
        baseCriDamage = Mgr.characterMgr.startCriDamage;
        maxBullet = Mgr.characterMgr.startBullet;
        nowBullet = Mgr.characterMgr.startBullet;

        gunBenchAct = false;

        //���� ��� ����
        ItemObjData weaponObjData = Mgr.itemMgr.CreateItemObjData(Mgr.characterMgr.startWeapon);
        if (weaponObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            weaponObjData.equip = true;
            weaponItem.Add(weaponObjData);
        }

        //�Ǽ��縮 ��� ����
        ItemObjData accessaryObjData = Mgr.itemMgr.CreateItemObjData(Mgr.characterMgr.startAccessary);
        if (accessaryObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            accessaryObjData.equip = true;
            accessaryItem.Add(accessaryObjData);
        }

        stageName = mapManager.mapNameKey;
    }
}
