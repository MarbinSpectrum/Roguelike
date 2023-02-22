using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 궤작오브젝트, 궤작에서 나오는 아이템과 나올 아이템이 결정된다.
////////////////////////////////////////////////////////////////////////////////
public abstract class Chest : MonoBehaviour
{
    public Obj chestType;
    public Vector2Int pos;
    public GameObject close;
    public GameObject open;

    private Item chestItem;
    [SerializeField]
    private List<Item> chestItemList = new List<Item>();


    public virtual void Init()
    {
        close.SetActive(true);
        open.SetActive(false);
        MakeChestItem();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 궤작에서 나올 아이템 생성 
    /// : 악세사리는 플레이어가 가지고있지 않는 아이템이 나오도록한다.
    ////////////////////////////////////////////////////////////////////////////////
    public virtual void MakeChestItem()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        HashSet<Item> hasItem = new HashSet<Item>();

        List<ItemObjData> nowAccessary = inventoryManager.GetItemList(ItemType.Accessary);
        foreach (ItemObjData accessary in nowAccessary)
        {
            //아이템들은 저주가 해제된 상태로 종류를 구분한다.
            //일단 아이템의 저주가 해제된 상태로 목록에 넣는다.
            Item unCurseItem = ItemManager.UnCurseItem(accessary.itemData.item);

            //현재 악세사리등록
            hasItem.Add(unCurseItem);
        }


        List<Item> itemList = new List<Item>();
        foreach(Item cItem in chestItemList)
        {
            //저주가 해제된 상태로 존재여부를 검사를 한다.
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

        //상자를 없앤다.
        close.SetActive(false);
        open.SetActive(true);
        chestManager.RemoveChestObj(pos);

        //상자 아이템을 생성한다.
        MakeChestItem();
        ItemManager itemManager = ItemManager.instance;
        itemManager.CreateItem(pos.x, pos.y, chestItem);
    }
}
