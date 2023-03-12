using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ���������� �����Ѵ�.
////////////////////////////////////////////////////////////////////////////////
public class ShopManager : DontDestroySingleton<ShopManager>
{
    //�⺻���� �߰��Ǵ� ������
    [SerializeField]
    private List<Item> defaultItem = new List<Item>();

    [Space(40)]

    //�߰��� �ĺ� ������
    [SerializeField]
    private List<Item> addItem = new List<Item>();

    //���� ������ ����Ʈ
    private List<ItemObjData> shopItemList = new List<ItemObjData>();

    private bool[] buyItem = new bool[8];

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �������� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateShopItem()
    {
        ItemManager itemManager = ItemManager.instance;
        shopItemList.Clear();
        for (int i = 0; i < ShopUI.SHOP_SLOT_NUM; i++)
            buyItem[i] = false;

        foreach (Item item in defaultItem)
        {
            ItemData itemData = itemManager.GetItemData(item);
            ItemObjData itemObjData = itemData.createItemObjData();

            shopItemList.Add(itemObjData);
        }

        int cnt = ShopUI.SHOP_SLOT_NUM - shopItemList.Count;

        List<int> randomList = MyLib.Algorithm.CreateRandomList(cnt,cnt);

        int nullCnt = 0;
        for (int i = 0; i < cnt; i++)
        {
            int r = Random.Range(0, addItem.Count);
            Item item = addItem[r];
            if(item == Item.Null)
            {
                nullCnt++;
                continue;
            }    
            ItemData itemData = itemManager.GetItemData(item);
            ItemObjData itemObjData = itemData.createItemObjData();

            shopItemList.Add(itemObjData);
        }

        shopItemList.Sort((x, y) =>
        {
            return x.itemData.item.CompareTo(y.itemData.item);
        });

        for(int i = 0; i < nullCnt; i++)
        {
            shopItemList.Add(null);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �����۵��� �����´�.
    ////////////////////////////////////////////////////////////////////////////////
    public List<ItemObjData> GetShopItemList()
    {
        return shopItemList;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : idx�� �ش��ϴ� �������� �����ߴ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsItemBuy(int idx)
    {
        if (idx < 0 || idx >= ShopUI.SHOP_SLOT_NUM)
            return false;
        return buyItem[idx];
    }
}
