using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : DontDestroySingleton<InventoryManager>
{
    public const uint weaponItemMax = 16;
    public const uint accessaryItemMax = 16;
    public const uint etcItemMax = 8;

    private List<ItemObjData> weaponItems = new List<ItemObjData>();
    private List<ItemObjData> accessaryItems = new List<ItemObjData>();
    private List<ItemObjData> etcItems = new List<ItemObjData>();

    [SerializeField]
    private SoundObj changeWeaponSE;
    [SerializeField]
    private SoundObj itemDrinkSE;

    public void ClearItemData()
    {
        weaponItems.Clear();
        accessaryItems.Clear();
        etcItems.Clear();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템을 인벤토리에 추가한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool AddItem(ItemObjData pItemObjData)
    {
        ItemType itemType = ItemManager.GetItemType(pItemObjData);
        switch (itemType)
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
    /// : pItemType에 해당하는 아이템 리스트를 반환
    ////////////////////////////////////////////////////////////////////////////////
    public List<ItemObjData> GetItemList(ItemType pItemType)
    {
        List<ItemObjData> itemList = null;
        switch (pItemType)
        {
            case ItemType.Weapon:
                itemList = weaponItems;
                break;
            case ItemType.Accessary:
                itemList = accessaryItems;
                break;
            case ItemType.Etc:
                itemList = etcItems;
                break;
        }

        if(itemList != null)
        {
            List<ItemObjData> equipItems = new List<ItemObjData>();
            List<ItemObjData> otherItems = new List<ItemObjData>();
            foreach(ItemObjData itemObj in itemList)
            {
                if (itemObj.equip)
                    equipItems.Add(itemObj);
                else
                    otherItems.Add(itemObj);
            }

            //아이템 목록을 정렬
            equipItems.Sort((x, y) =>
            {
                return x.itemData.item.CompareTo(y.itemData.item);
            });
            otherItems.Sort((x, y) =>
            {
                return x.itemData.item.CompareTo(y.itemData.item);
            });
            itemList.Clear();
            foreach (ItemObjData itemObj in equipItems)
                itemList.Add(itemObj);
            foreach (ItemObjData itemObj in otherItems)
                itemList.Add(itemObj);
        }

        return itemList;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 현재 장착중인 무기를 보여준다.
    ////////////////////////////////////////////////////////////////////////////////
    public ItemObjData NowWeapon()
    {
        ItemObjData nowWeapon = null;
        foreach (ItemObjData itemObjData in weaponItems)
        {
            if (itemObjData.equip)
            {
                nowWeapon = itemObjData;
                break;
            }
        }
        return nowWeapon;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 현재 장착중인 악세사리를 보여준다.
    ////////////////////////////////////////////////////////////////////////////////
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemType에 해당하는 pIdx의 아이템을 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveItem(ItemType pItemType, int pIdx)
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pIdx의 아이템 사용 
    /// : 사용 아이템은 기타 아이템에 속해있다.
    /// : 기타아이템 리스트에 해당하는 아이템을 사용한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UseItem(int pIdx)
    {
        if (etcItems.Count <= pIdx)
            return;

        ItemObjData itemObjData = etcItems[pIdx];
        ItemManager.RunUseItemEffect(itemObjData);

        if(ItemManager.IsDrink(itemObjData.itemData.item))
        {
            itemDrinkSE.PlaySE();
        }

        itemObjData.count--;
        if (itemObjData.count == 0)
        {
            //아이템을 모두 사용했다.
            //목록에서 제거한다.
            etcItems.RemoveAt(pIdx);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어의 장비를 pItemObjData에 해당하는 장비로 교체한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool ChangeItem(ref ItemObjData pItemObjData)
    {
        if (pItemObjData == null)
        {
            //현재 장비가 없는것 같다.
            return false;
        }

        ItemObjData nowItemData = null;
        ItemType itemType = ItemManager.GetItemType(pItemObjData);
        if (itemType == ItemType.Weapon)
        {
            changeWeaponSE.PlaySE();
            nowItemData = NowWeapon();
        }
        else if (itemType == ItemType.Accessary)
        {
            //악세사리는 다른 장비를 바로끼는 로직이 아니다.
            //하지만 구현은 해두겠다.
            List<ItemObjData> nowAccessary = NowAccessaryList();
            if (nowAccessary.Count > 0)
            {
                //0번째 아이템을 갈아낀다.
                nowItemData = nowAccessary[0];
            }
        }
        else
        {
            //해당 장비는 못끼는 장비같다.
            return false;
        }

        if (nowItemData != null)
        {
            bool cantTakeOff = ItemManager.CantTakeOff(nowItemData);
            if (cantTakeOff)
            {
                //벗을수 없는 장비다.
                return false;
            }

            //현재 장비를 벗는다..
            nowItemData.equip = false;
        }

        //해당 장비를 장착한다.
        pItemObjData.equip = true;

        return true;
    }

    public bool TakeOffItem(ref ItemObjData pItemObjData)
    {
        if (pItemObjData == null)
        {
            //현재 장비가 없는것 같다.
            return false;
        }

        pItemObjData.equip = false;

        return true;
    }
}
