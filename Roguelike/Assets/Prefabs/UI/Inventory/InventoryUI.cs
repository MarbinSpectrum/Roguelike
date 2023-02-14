using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �κ��丮UI�� ����
////////////////////////////////////////////////////////////////////////////////
public class InventoryUI : MonoBehaviour
{
    #region[private bool isRun]
    private bool IsRun;
    public bool isRun
    {
        get
        {
            return IsRun;
        }
        set
        {
            IsRun = value;
        }

    }
    #endregion

    [SerializeField]
    private GameObject uiObj;

    const uint itemSlotMax = 16;
    const uint weaponItemMax = 16;
    const uint accessaryItemMax = 16;
    const uint etcItemMax = 8;

    public ItemType actCategory = ItemType.Etc;

    [SerializeField]
    private UpdateSlotImg updateSlotImg;
    [SerializeField]
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    private List<ItemObjData> weaponItems = new List<ItemObjData>();
    private List<ItemObjData> accessaryItems = new List<ItemObjData>();
    private List<ItemObjData> etcItems = new List<ItemObjData>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : �κ��丮 ��Ȱ��/Ȱ��
    ////////////////////////////////////////////////////////////////////////////////
    public void ActInventory()
    {
        ActInventory(!isRun);
    }
    public void ActInventory(bool pState)
    {
        TotalUI totalUI = TotalUI.instance;
        totalUI.ActKeyPad(!pState);

        isRun = pState;
        uiObj.SetActive(isRun);
        UpdateSlot(actCategory);
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : �������� �κ��丮�� �߰��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool AddItem(ItemObjData pItemObjData)
    {
        ItemType itemType = ItemManager.GetItemType(pItemObjData);
        switch(itemType)
        {
            case ItemType.Weapon:
                return AddItem(ref weaponItems, pItemObjData, weaponItemMax);
            case ItemType.Accessary:
                return AddItem(ref accessaryItems, pItemObjData, accessaryItemMax);
            case ItemType.Etc:
                return AddItem(ref etcItems, pItemObjData, etcItemMax);
        }
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjs�� �ش��ϴ� ����Ʈ�� pItemObj�� ������ ���� �������� �߰��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    private bool AddItem(ref List<ItemObjData> pItemObjDatas, ItemObjData pItemObjData, uint pSlotMax)
    {
        ItemData AddItemData = pItemObjData.itemData;

        //������ �������̸� ������ �߰����ش�.
        foreach (ItemObjData itemObjData in pItemObjDatas)
        {
            ItemData invenItemData = itemObjData.itemData;
            if (AddItemData.stockItem && invenItemData.item == AddItemData.item)
            {
                itemObjData.count += pItemObjData.count;
                return true;
            }
        }

        if (pItemObjDatas.Count >= pSlotMax)
            return false;

        //������ ��ü�� �����ϰ� �߰�
        pItemObjDatas.Add(new ItemObjData(pItemObjData));


        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������  ������ ī�װ��� �´°����� ������Ʈ
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateSlot(ItemType pItemType)
    {
        actCategory = pItemType;
        switch (pItemType)
        {
            case ItemType.Weapon:
                UpdateSlot(ref weaponItems, 1, weaponItemMax);
                break;
            case ItemType.Accessary:
                UpdateSlot(ref accessaryItems, CharacterManager.MAX_ACCESSARY_SLOT, accessaryItemMax);
                break;
            case ItemType.Etc:
                UpdateSlot(ref etcItems, 0, etcItemMax);
                break;
        }
    }
    private void UpdateSlot(ref List<ItemObjData> pItemObjDatas,uint pEquipSize, uint pSlotMax)
    {
        //������ ����� ����
        pItemObjDatas.Sort((x, y) =>
        {
            int ret1 = x.equip ? -1 : 1;
            int ret2 = ret1 != 0 ? ret1 : x.itemData.item.CompareTo(y.itemData.item);
            return ret2;
        });

        updateSlotImg.equipSlotSize = (int)pEquipSize;

        int idx = 0;
        ItemObjData itemObjData = null;
        for (int i = 0; i < itemSlotMax; i++)
        {
            if (itemObjData == null)
            {
                if(pItemObjDatas.Count > idx)
                {
                    //������ �������� ã�´�.
                    itemObjData = pItemObjDatas[idx];
                    idx++;
                }
            }

            if (i < pSlotMax)
            {
                //�ش�ĭ�� ������ �����Ѵ�.
                itemSlots[i].gameObject.SetActive(true);
            }
            else
            {
                //�ش�ĭ�� ������ ����.
                itemSlots[i].gameObject.SetActive(false);
                continue;
            }


            if (itemObjData == null || (i < pEquipSize && itemObjData != null && itemObjData.equip == false))            
            {
                //�ش�ĭ�� ������ ���� ����϶� ǥ�������ʴ´�.
                //1.ǥ���� �������� ������
                //2.���� �����ε� ���� ǥ���Ұ��� �������������϶�
                itemSlots[i].slotSprite = null;
                itemSlots[i].slotNum = 0;
            }
            else if (itemObjData != null)
            {
                //�ش�ĭ�� �������� �����ش�.
                ItemData itemData = itemObjData.itemData;
                itemSlots[i].item = itemObjData.itemData.item;
                itemSlots[i].slotSprite = itemData.itemSprite_UI;
                itemSlots[i].slotNum = (uint)itemObjData.count;
                itemObjData = null;
            }
        }
    }

    public ItemObjData NowWeapon()
    {
        ItemObjData nowWeapon = null;
        foreach(ItemObjData itemObjData in weaponItems)
        {
            if(itemObjData.equip)
            {
                nowWeapon = itemObjData;
                break;
            }
        }
        return nowWeapon;
    }

    public List<ItemObjData> NowAccessaryList()
    {
        List<ItemObjData> nowAccessaryList = new List<ItemObjData>();
        foreach (ItemObjData itemObjData in accessaryItems)
        {
            if (itemObjData.equip)
            {
                //���� �������� �Ǽ��縮 ��� List�� ����Ѵ�.
                nowAccessaryList.Add(itemObjData);
            }
        }
        return nowAccessaryList;
    }
    public List<ItemObjData> HasAccessaryList()
    {
        return accessaryItems;
    }

    public void RemoveItem(ItemType pItemType,int pIdx)
    {
        List<ItemObjData> itemObjDatas = null;
        switch (pItemType)
        {
            case ItemType.Weapon:
                itemObjDatas = weaponItems;
                break;
            case ItemType.Accessary:
                itemObjDatas = accessaryItems;
                break;
            case ItemType.Etc:
                itemObjDatas = etcItems;
                break;
        }
        itemObjDatas.RemoveAt(pIdx);
    }

    public void ShowItemData(int pIdx)
    {
        List<ItemObjData> itemObjDatas = null;
        switch (actCategory)
        {
            case ItemType.Weapon:
                itemObjDatas = weaponItems;
                break;
            case ItemType.Accessary:
                itemObjDatas = accessaryItems;
                int nowEquipSize = CharacterManager.instance.NowAccessaryList().Count;
                if(pIdx <= nowEquipSize)
                {
                    //�ε��� ���� ������ �ʿ䰡 ���ºκ�
                    //�ش� ��ġ�� �ش��ϴ� �������� �������� �������̴�.
                }
                else if (nowEquipSize <= pIdx && pIdx < CharacterManager.MAX_ACCESSARY_SLOT)
                {
                    //����������ġ�߿��� ���°����� ǥ�þ��Ѵ�.
                    return;
                }
                else if (pIdx >= CharacterManager.MAX_ACCESSARY_SLOT)
                {
                    //���������� �ƴ� ���� �������� ǥ���ϴºκ�
                    //���� ���� ������ ����ؼ� �ε������� �����Ѵ�.
                    pIdx -= (CharacterManager.MAX_ACCESSARY_SLOT - nowEquipSize);
                }
                break;
            case ItemType.Etc:
                itemObjDatas = etcItems;
                break;
        }
        if (itemObjDatas == null)
            return;

        if (itemObjDatas.Count <= pIdx)
            return;

        TotalUI totalUI = TotalUI.instance;
        totalUI.ShowItemData(itemObjDatas[pIdx], pIdx);
    }
}
