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
    const uint armorItemMax = 16;
    const uint etcItemMax = 8;

    public ItemType actCategory = ItemType.Etc;

    [SerializeField]
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    private List<ItemObjData> weaponItems = new List<ItemObjData>();
    private List<ItemObjData> armorItems = new List<ItemObjData>();
    private List<ItemObjData> etcItems = new List<ItemObjData>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : 인벤토리 비활성/활성
    ////////////////////////////////////////////////////////////////////////////////
    public void ActInventory()
    {
        isRun = !isRun;
        uiObj.SetActive(isRun);
        UpdateSlot(actCategory);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템을 인벤토리에 추가한다.
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
    /// : pItemObjs에 해당하는 리스트에 pItemObj의 정보를 토대로 아이템을 추가한다.
    ////////////////////////////////////////////////////////////////////////////////
    private bool AddItem(ref List<ItemObjData> pItemObjDatas,ItemObj pItemObj,uint pSlotMax)
    {
        ItemObjData AddItemObjData = pItemObj.itemObjData;
        ItemData AddItemData = AddItemObjData.itemData;

        //스톡형 아이템이면 스톡을 추가해준다.
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

        //아이템 객체를 생성하고 추가
        pItemObjDatas.Add(new ItemObjData(pItemObj.itemObjData));

        //아이템 목록을 정렬
        pItemObjDatas.Sort(new Comparison<ItemObjData>(
                (n1, n2) => n2.itemData.item.CompareTo(n1.itemData.item)));
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
                //해당카은 슬롯이 존재한다.
                itemSlots[i].gameObject.SetActive(true);
            }
            else
            {
                //해당칸은 슬롯이 없다.
                itemSlots[i].gameObject.SetActive(false);
                continue;
            }

            if(itemObj != null)
            {
                //해당칸의 아이템을 보여준다.
                ItemData itemData = itemObj.itemData;
                itemSlots[i].slotSprite = itemData.itemSprite_UI;
                itemSlots[i].slotNum = (uint)itemObj.count;
            }
            else
            {
                //해당칸에 아이템이 없다 표시하지않는다.
                itemSlots[i].slotSprite = null;
                itemSlots[i].slotNum = 0;
            }
        }
    }
}
