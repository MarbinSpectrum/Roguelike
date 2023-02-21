using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ���ۿ�����Ʈ, ���ۿ��� ������ �����۰� ���� �������� �����ȴ�.
////////////////////////////////////////////////////////////////////////////////
public class Chest : MonoBehaviour
{
    public Vector2Int pos;
    public GameObject close;
    public GameObject open;
    private Item chestItem;
    [SerializeField]
    private List<Item> chestItemList = new List<Item>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ۿ��� ���� ������ ���� 
    /// : �Ǽ��縮�� �÷��̾ ���������� �ʴ� �������� ���������Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public virtual void MakeChestItem()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        HashSet<Item> hasItem = new HashSet<Item>();

        List<ItemObjData> nowAccessary = inventoryManager.GetItemList(ItemType.Accessary);
        foreach (ItemObjData accessary in nowAccessary)
        {
            //�����۵��� ���ְ� ������ ���·� ������ �����Ѵ�.
            //�ϴ� �������� ���ְ� ������ ���·� ��Ͽ� �ִ´�.
            Item unCurseItem = ItemManager.UnCurseItem(accessary.itemData.item);

            //���� �Ǽ��縮���
            hasItem.Add(unCurseItem);
        }


        List<Item> itemList = new List<Item>();
        foreach(Item cItem in chestItemList)
        {
            //���ְ� ������ ���·� ���翩�θ� �˻縦 �Ѵ�.
            Item unCurseItem = ItemManager.UnCurseItem(cItem);
            if (hasItem.Contains(unCurseItem))
            {
                itemList.Add(Item.Coin);
                continue;
            }
            itemList.Add(cItem);
        }

        int r = Random.Range(0, itemList.Count);
        chestItem = itemList[r];
    }

    public virtual void RemoveChestObj()
    {
        ChestManager chestManager = ChestManager.instance;

        //���ڸ� ���ش�.
        close.SetActive(false);
        open.SetActive(true);
        chestManager.RemoveChestObj(pos);

        //���� �������� �����Ѵ�.
        MakeChestItem();
        ItemManager itemManager = ItemManager.instance;
        itemManager.CreateItem(pos.x, pos.y, chestItem);
    }
}
