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

    ////////////////////////////////////////////////////////////////////////////////
    /// : �������� �κ��丮�� �߰��Ѵ�.
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
    /// : pItemType�� �ش��ϴ� ������ ����Ʈ�� ��ȯ
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

            //������ ����� ����
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
    /// : ���� �������� ���⸦ �����ش�.
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
    /// : ���� �������� �Ǽ��縮�� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemType�� �ش��ϴ� pIdx�� �������� ����
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
    /// : pIdx�� ������ ��� 
    /// : ��� �������� ��Ÿ �����ۿ� �����ִ�.
    /// : ��Ÿ������ ����Ʈ�� �ش��ϴ� �������� ����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void UseItem(int pIdx)
    {
        if (etcItems.Count <= pIdx)
            return;

        ItemObjData itemObjData = etcItems[pIdx];
        ItemManager.RunUseItemEffect(itemObjData);

        itemObjData.count--;
        if (itemObjData.count == 0)
        {
            //�������� ��� ����ߴ�.
            //��Ͽ��� �����Ѵ�.
            etcItems.RemoveAt(pIdx);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾��� ��� pItemObjData�� �ش��ϴ� ���� ��ü�Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool ChangeItem(ref ItemObjData pItemObjData)
    {
        if (pItemObjData == null)
        {
            //���� ��� ���°� ����.
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
            //�Ǽ��縮�� �ٸ� ��� �ٷγ��� ������ �ƴϴ�.
            //������ ������ �صΰڴ�.
            List<ItemObjData> nowAccessary = NowAccessaryList();
            if (nowAccessary.Count > 0)
            {
                //0��° �������� ���Ƴ���.
                nowItemData = nowAccessary[0];
            }
        }
        else
        {
            //�ش� ���� ������ ��񰰴�.
            return false;
        }

        if (nowItemData != null)
        {
            bool cantTakeOff = ItemManager.CantTakeOff(nowItemData.itemData.item);
            if (cantTakeOff)
            {
                //������ ���� ����.
                return false;
            }

            //���� ��� ���´�..
            nowItemData.equip = false;
        }

        //�ش� ��� �����Ѵ�.
        pItemObjData.equip = true;

        return true;
    }

    public bool TakeOffItem(ref ItemObjData pItemObjData)
    {
        if (pItemObjData == null)
        {
            //���� ��� ���°� ����.
            return false;
        }

        pItemObjData.equip = false;

        return true;
    }
}
