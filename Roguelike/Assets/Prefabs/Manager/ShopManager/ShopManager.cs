using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 상점정보를 관리한다.
////////////////////////////////////////////////////////////////////////////////
public class ShopManager : DontDestroySingleton<ShopManager>
{
    //기본으로 추가되는 아이템
    [SerializeField]
    private List<Item> defaultItem = new List<Item>();

    [Space(40)]

    //추가될 후보 아이템
    [SerializeField]
    private List<Item> addItem = new List<Item>();

    //상점 아이템 리스트
    private List<ItemObjData> shopItemList = new List<ItemObjData>();

    private bool[] buyItem = new bool[8];

    ////////////////////////////////////////////////////////////////////////////////
    /// : 상점의 아이템을 생성한다.
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
    /// : 상점의 아이템들을 가져온다.
    ////////////////////////////////////////////////////////////////////////////////
    public List<ItemObjData> GetShopItemList()
    {
        return shopItemList;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : idx에 해당하는 아이템을 구매했는지 확인한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsItemBuy(int idx)
    {
        if (idx < 0 || idx >= ShopUI.SHOP_SLOT_NUM)
            return false;
        return buyItem[idx];
    }
}
