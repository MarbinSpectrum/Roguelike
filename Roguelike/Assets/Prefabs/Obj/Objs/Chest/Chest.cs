using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 궤작오브젝트, 궤작에서 나오는 아이템과 나올 아이템이 결정된다.
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
    /// : 궤작에서 나올 아이템 생성 
    /// : 악세사리는 플레이어가 가지고있지 않는 아이템이 나오도록한다.
    ////////////////////////////////////////////////////////////////////////////////
    public virtual void MakeChestItem()
    {
        CharacterManager characterManager = CharacterManager.instance;
        HashSet<Item> hasItem = new HashSet<Item>();

        List<ItemObjData> nowAccessary = characterManager.HasAccessaryList();
        foreach (ItemObjData accessary in nowAccessary)
        {   
            //현재 악세사리등록
            hasItem.Add(accessary.itemData.item);
        }


        List<Item> itemList = new List<Item>();
        foreach(Item cItem in chestItemList)
        {
            if (hasItem.Contains(cItem))
                continue;
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
