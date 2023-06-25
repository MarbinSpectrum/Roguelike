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
    //4.�̺�Ʈ Ȱ��ȭ����
    //5.�õ�

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // �÷��̾� ����
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
    // �κ��丮 ������
    public List<ItemObjData> etcItem = new List<ItemObjData>();
    public List<ItemObjData> weaponItem = new List<ItemObjData>();
    public List<ItemObjData> accessaryItem = new List<ItemObjData>();

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // ���� ��������
    public string stageName;
    public List<int> visitRoomIdx = new List<int>();    //�湮�ߴ� �� Idx

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // �̺�Ʈ Ȱ��ȭ����
    public bool gunBenchAct;

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // �õ�
    public int randomSeed;


    /////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : InventoryManager�� CharacterManager�� ���ؼ� �÷��̾� ������ �����Ѵ�.
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

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        stageName = mapManager.mapNameKey;
        visitRoomIdx = new List<int>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        gunBenchAct = false;

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        randomSeed = (int)(Time.time) % 10000;
    }
}
