using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 인벤토리UI를 관리
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
    /// : 인벤토리 비활성/활성
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
    /// : 아이템  슬롯을 카테고리에 맞는것으로 업데이트
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
                    //보여줄 아이템을 찾는다.
                    itemObjData = pItemObjDatas[idx];
                    idx++;
                }
            }

            if (i < pSlotMax)
            {
                //해당칸은 슬롯이 존재한다.
                itemSlots[i].gameObject.SetActive(true);
            }
            else
            {
                //해당칸은 슬롯이 없다.
                itemSlots[i].gameObject.SetActive(false);
                continue;
            }


            if (itemObjData == null || (i < pEquipSize && itemObjData != null && itemObjData.equip == false))            
            {
                //해당칸은 다음과 같은 경우일때 표시하지않는다.
                //1.표시할 아이템이 없을때
                //2.장착 슬롯인데 현재 표시할것이 비장착아이템일때
                itemSlots[i].slotSprite = null;
                itemSlots[i].slotNum = 0;
            }
            else if (itemObjData != null)
            {
                //해당칸의 아이템을 보여준다.
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
                    //인덱스 값을 조정할 필요가 없는부분
                    //해당 위치에 해당하는 아이템은 장착중인 아이템이다.
                }
                else if (nowEquipSize <= pIdx && pIdx < CharacterManager.MAX_ACCESSARY_SLOT)
                {
                    //장착슬롯위치중에서 남는공간은 표시안한다.
                    return;
                }
                else if (pIdx >= CharacterManager.MAX_ACCESSARY_SLOT)
                {
                    //장착슬롯이 아닌 곳의 아이템을 표시하는부분
                    //남는 슬롯 공간을 고려해서 인덱스값을 조정한다.
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
