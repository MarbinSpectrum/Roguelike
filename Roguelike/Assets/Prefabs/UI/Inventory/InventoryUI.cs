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
    const uint armorItemMax = 16;
    const uint etcItemMax = 8;

    public ItemType actCategory = ItemType.Etc;

    [SerializeField]
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    private List<ItemObjData> weaponItems = new List<ItemObjData>();
    private List<ItemObjData> armorItems = new List<ItemObjData>();
    private List<ItemObjData> etcItems = new List<ItemObjData>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : �κ��丮 ��Ȱ��/Ȱ��
    ////////////////////////////////////////////////////////////////////////////////
    public void ActInventory()
    {
        isRun = !isRun;
        uiObj.SetActive(isRun);
        UpdateSlot(actCategory);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �������� �κ��丮�� �߰��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool AddItem(ItemObj pItemObj)
    {
        ItemManager itemManager = ItemManager.instance;

        ItemType itemType = itemManager.GetItemType(pItemObj);
        switch(itemType)
        {
            case ItemType.Weapon:
                return AddItem(ref weaponItems, pItemObj,weaponItemMax);
            case ItemType.Armor:
                return AddItem(ref armorItems, pItemObj,armorItemMax);
            case ItemType.Etc:
                return AddItem(ref etcItems, pItemObj, etcItemMax);
        }
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjs�� �ش��ϴ� ����Ʈ�� pItemObj�� ������ ���� �������� �߰��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    private bool AddItem(ref List<ItemObjData> pItemObjDatas,ItemObj pItemObj,uint pSlotMax)
    {
        ItemObjData AddItemObjData = pItemObj.itemObjData;
        ItemData AddItemData = AddItemObjData.itemData;

        //������ �������̸� ������ �߰����ش�.
        foreach (ItemObjData itemObjData in pItemObjDatas)
        {
            ItemData invenItemData = itemObjData.itemData;
            if (AddItemData.stockItem && invenItemData.item == AddItemData.item)
            {
                itemObjData.count += AddItemObjData.count;
                return true;
            }
        }

        if (pItemObjDatas.Count >= pSlotMax)
            return false;

        //������ ��ü�� �����ϰ� �߰�
        pItemObjDatas.Add(new ItemObjData(pItemObj.itemObjData));

        //������ ����� ����
        pItemObjDatas.Sort(new Comparison<ItemObjData>(
                (n1, n2) => n2.itemData.item.CompareTo(n1.itemData.item)));
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
            case ItemType.Armor:
                UpdateSlot(ref armorItems,armorItemMax);
                break;
            case ItemType.Etc:
                UpdateSlot(ref etcItems,etcItemMax);
                break;
        }
    }
    private void UpdateSlot(ref List<ItemObjData> pItemObjDatas,uint pSlotMax)
    {
        for(int i = 0; i < itemSlotMax; i++)
        {
            ItemObjData itemObj = null;
            if(pItemObjDatas.Count > i)
            {
                itemObj = pItemObjDatas[i];
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

            if(itemObj != null)
            {
                //�ش�ĭ�� �������� �����ش�.
                ItemData itemData = itemObj.itemData;
                itemSlots[i].slotSprite = itemData.itemSprite_UI;
                itemSlots[i].slotNum = (uint)itemObj.count;
            }
            else
            {
                //�ش�ĭ�� �������� ���� ǥ�������ʴ´�.
                itemSlots[i].slotSprite = null;
                itemSlots[i].slotNum = 0;
            }
        }
    }
}
