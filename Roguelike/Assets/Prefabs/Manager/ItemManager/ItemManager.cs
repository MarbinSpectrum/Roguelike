using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : �������� ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////

public class ItemManager : FieldObjectSingleton<ItemManager>
{
    [SerializeField]
    private List<ItemData> itemDataList = new List<ItemData>();
    private Dictionary<Vector2Int, ItemObj> items = new Dictionary<Vector2Int, ItemObj>();
    private Dictionary<Item, ItemObj> itemObjs;

    private void Init()
    {
        if (itemObjs == null)
        {
            itemObjs = new Dictionary<Item, ItemObj>();
            foreach (ItemData itemData in itemDataList)
            {
                itemObjs[itemData.item] = itemData.itemObj;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsItem(int pX,int pY)
    {
        ItemObj itemObj = GetItem(pX, pY);
        if (itemObj == null)
            return false;
        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �޾ƿ´�.
    ////////////////////////////////////////////////////////////////////////////////
    public ItemObj GetItem(int pX,int pY)
    {
        ItemObj itemObj = null;
        if (items.ContainsKey(new Vector2Int(pX, pY)))
            itemObj = items[new Vector2Int(pX, pY)];
        return itemObj;
    }

    public void CreateItem(int pX, int pY,Item pItem)
    {
        Init();

        if (IsItem(pX, pY))
            return;
        if (itemObjs.ContainsKey(pItem) == false)
            return;
        if (itemObjs[pItem] == null)
            return;
        ItemObj itemObj = Instantiate(itemObjs[pItem]);
        itemObj.transform.position =
            new Vector3(CreateMap.tileSize * pX, CreateMap.tileSize * pY, 0);
    }
}
