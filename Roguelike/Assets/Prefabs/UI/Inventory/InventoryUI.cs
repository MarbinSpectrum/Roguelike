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
    /// : 아이템을 인벤토리에 추가한다.
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
    /// : pItemObjs에 해당하는 리스트에 pItemObj의 정보를 토대로 아이템을 추가한다.
    ////////////////////////////////////////////////////////////////////////////////
    private bool AddItem(ref List<ItemObjData> pItemObjDatas, ItemObjData pItemObjData, uint pSlotMax)
    {
        ItemData AddItemData = pItemObjData.itemData;

        //스톡형 아이템이면 스톡을 추가해준다.
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

        //아이템 객체를 생성하고 추가
        pItemObjDatas.Add(new ItemObjData(pItemObjData));


        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템  슬롯을 카테고리에 맞는것으로 업데이트
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
        //아이템 목록을 정렬
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
                //현재 장착중인 악세사리 장비를 List에 등록한다.
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
