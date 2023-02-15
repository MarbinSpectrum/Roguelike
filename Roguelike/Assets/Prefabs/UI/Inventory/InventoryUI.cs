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

    public ItemType actCategory = ItemType.Etc;

    [SerializeField]
    private UpdateSlotImg updateSlotImg;
    [SerializeField]
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

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
    /// : ������  ������ ī�װ��� �´°����� ������Ʈ
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateSlot(ItemType pItemType)
    {
        InventoryManager inventoryManager = InventoryManager.instance;

        actCategory = pItemType;
        List<ItemObjData> itemList = inventoryManager.GetItemList(pItemType);
        switch (pItemType)
        {
            case ItemType.Weapon:
                UpdateSlot(ref itemList, 1, InventoryManager.weaponItemMax);
                break;
            case ItemType.Accessary:
                UpdateSlot(ref itemList, CharacterManager.MAX_ACCESSARY_SLOT, InventoryManager.accessaryItemMax);
                break;
            case ItemType.Etc:
                UpdateSlot(ref itemList, 0, InventoryManager.etcItemMax);
                break;
        }
    }
    private void UpdateSlot(ref List<ItemObjData> pItemObjDatas,uint pEquipSize, uint pSlotMax)
    {
        updateSlotImg.equipSlotSize = (int)pEquipSize;

        int idx = 0;
        ItemObjData itemObjData = null;
        for (int i = 0; i < itemSlots.Count; i++)
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

    public void ShowItemData(int pIdx)
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        List<ItemObjData> itemObjDatas = inventoryManager.GetItemList(actCategory);
        switch (actCategory)
        {
            case ItemType.Weapon:
                break;
            case ItemType.Accessary:
                int nowEquipSize = inventoryManager .NowAccessaryList().Count;
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
