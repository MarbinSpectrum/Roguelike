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
                UpdateSlot(ref weaponItems,weaponItemMax);
                break;
            case ItemType.Accessary:
                UpdateSlot(ref accessaryItems, accessaryItemMax);
                break;
            case ItemType.Etc:
                UpdateSlot(ref etcItems,etcItemMax);
                break;
        }
    }
    private void UpdateSlot(ref List<ItemObjData> pItemObjDatas, uint pSlotMax)
    {
        //������ ����� ����
        pItemObjDatas.Sort((x, y) =>
        {
            int ret1 = x.equip ? -1 : 1;
            int ret2 = ret1 != 0 ? ret1 : x.itemData.item.CompareTo(y.itemData.item);
            return ret2;
        });

        for (int i = 0; i < itemSlotMax; i++)
        {
            ItemObjData itemObjData = null;
            if(pItemObjDatas.Count > i)
            {
                itemObjData = pItemObjDatas[i];
            }

            if(i < pSlotMax)
            {
                //�ش�ī�� ������ �����Ѵ�.
                itemSlots[i].gameObject.SetActive(true);
            }
            else
            {
                //�ش�ĭ�� ������ ����.
                itemSlots[i].gameObject.SetActive(false);
                continue;
            }

            if (itemObjData != null)
            {
                //�ش�ĭ�� �������� �����ش�.
                ItemData itemData = itemObjData.itemData;
                itemSlots[i].item = itemObjData.itemData.item;
                itemSlots[i].slotSprite = itemData.itemSprite_UI;
                itemSlots[i].slotNum = (uint)itemObjData.count;
                itemSlots[i].isEquip = itemObjData.equip;
            }
            else
            {
                //�ش�ĭ�� �������� ���� ǥ�������ʴ´�.
                itemSlots[i].slotSprite = null;
                itemSlots[i].slotNum = 0;
                itemSlots[i].isEquip = false;
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

    public ItemObjData NowAccessary()
    {
        ItemObjData nowAccessary = null;
        foreach (ItemObjData itemObjData in accessaryItems)
        {
            if (itemObjData.equip)
            {
                nowAccessary = itemObjData;
                break;
            }
        }
        return nowAccessary;
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
