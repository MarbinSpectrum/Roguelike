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
    //4.이벤트 활성화여부
    //5.시드

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // 플레이어 스텟
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

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // 인벤토리 아이템
    public List<ItemObjData> etcItem = new List<ItemObjData>();
    public List<ItemObjData> weaponItem = new List<ItemObjData>();
    public List<ItemObjData> accessaryItem = new List<ItemObjData>();

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // 현재 스테이지
    public string stageName;
    public List<int> visitRoomIdx = new List<int>();    //방문했던 방 Idx

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // 이벤트 활성화여부
    public bool gunBenchAct;

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // 시드
    public int randomSeed;


    /////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : InventoryManager와 CharacterManager를 통해서 플레이어 정보를 생성한다.
    /////////////////////////////////////////////////////////////////////////////////////////////////////
    public PlayData(int pSave)
    {
        MapManager mapManager = MapManager.instance;

        level = Mgr.characterMgr.nowLevel;
        baseMaxHp = Mgr.characterMgr.baseMaxHp;
        nowHp = Mgr.characterMgr.nowHp;
        maxExp = Mgr.characterMgr.maxExp;
        nowExp = Mgr.characterMgr.nowExp;
        basePow = Mgr.characterMgr.basePow;
        baseBalance = Mgr.characterMgr.baseBalance;
        baseCriPer = Mgr.characterMgr.baseCriPer;
        baseCriDamage = Mgr.characterMgr.baseCriDamage;
        maxBullet = Mgr.characterMgr.maxBullet;
        nowBullet = Mgr.characterMgr.nowBullet;
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        List<ItemObjData> etcitemList = Mgr.inventoryMgr.GetItemList(ItemType.Etc);
        etcItem = etcitemList;
        List<ItemObjData> weaponitemList = Mgr.inventoryMgr.GetItemList(ItemType.Weapon);
        weaponItem = weaponitemList;
        List<ItemObjData> accessaritemList = Mgr.inventoryMgr.GetItemList(ItemType.Accessary);
        accessaryItem = accessaritemList;

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        stageName = mapManager.mapNameKey;
        visitRoomIdx = mapManager.GetVisitRoomIdx();

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        gunBenchAct = GameManager.gunBenchAct;

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        randomSeed = Random.seed;
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //무기 장비 설정
        ItemObjData weaponObjData = Mgr.itemMgr.CreateItemObjData(Mgr.characterMgr.startWeapon);
        if (weaponObjData != null)
        {
            //해당 장비를 장착한다.
            weaponObjData.equip = true;
            weaponItem.Add(weaponObjData);
        }

        //악세사리 장비 설정
        ItemObjData accessaryObjData = Mgr.itemMgr.CreateItemObjData(Mgr.characterMgr.startAccessary);
        if (accessaryObjData != null)
        {
            //해당 장비를 장착한다.
            accessaryObjData.equip = true;
            accessaryItem.Add(accessaryObjData);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        stageName = mapManager.mapNameKey;
        visitRoomIdx = new List<int>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        gunBenchAct = false;

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        randomSeed = (int)(Time.time) % 10000;
    }
}
