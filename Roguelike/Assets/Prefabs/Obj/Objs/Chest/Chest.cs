using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Vector2Int pos;
    public GameObject close;
    public GameObject open;
    private Item chestItem;
    [SerializeField]
    private List<Item> chestItemList = new List<Item>();

    public virtual void MakeChestItem()
    {
        int r = Random.Range(0, chestItemList.Count);
        Item cItem = chestItemList[r];
        chestItem = cItem;
    }

    public virtual void RemoveChestObj()
    {
        ChestManager chestManager = ChestManager.instance;
        close.SetActive(false);
        open.SetActive(true);
        chestManager.RemoveChestObj(pos);

        ItemManager itemManager = ItemManager.instance;
        itemManager.CreateItem(pos.x, pos.y, chestItem);
    }
}
