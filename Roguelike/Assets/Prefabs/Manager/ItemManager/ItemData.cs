using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 4)]
public class ItemData : ScriptableObject
{
    public Item item;
    public ItemObj itemObj;
    public Sprite itemSprite_UI;

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Etc")]
    public bool stockItem;
    [ShowIf("@ItemManager.GetItemType(item) != ItemType.Weapon && stockItem"),MinMaxSlider(0, 100)]
    public Vector2Int valueMinMax;

    [ShowIf("@ItemManager.GetItemType(item) != ItemType.Etc || ItemManager.IsUseItem(item)")]
    [BoxGroup("Box")]
    public List<ItemStatData> itemStatDatas = new List<ItemStatData>();

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public Sprite frontSprite;

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public Sprite sideSprite;

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public uint range;

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public uint reloadDelay;

    [Space(100)]
    public string nameKey;
    [ShowIf("@ItemManager.GetItemType(item) != ItemType.Weapon")]
    public string explainKey;

    public ItemObjData createItemObjData()
    {
        ItemObjData itemObjData = new ItemObjData();
        itemObjData.itemData = this;
        itemObjData.item = item;

        //스텟을 초기화
        foreach (ItemStatData itemStatData in itemStatDatas)
            itemStatData.MakeDataValue();

        ItemType itemType = ItemManager.GetItemType(item);

        if (itemType == ItemType.Etc && itemObjData.itemData.stockItem)
        {
            //스톡이 되는 아이템은 랜덤한 범위내에서 아이템 갯수 설정
            itemObjData.count = Random.Range(
                itemObjData.itemData.valueMinMax.x,
                itemObjData.itemData.valueMinMax.y);
        }
        else
        {
            //아니라면 무조건 한개임
            itemObjData.count = 1;
        }

        if(itemType != ItemType.Etc || ItemManager.IsUseItem(item))
        {
            //스탯정보를 대입
            foreach (ItemStatData itemStatData in itemStatDatas)
                itemObjData.itemStats.Add(new ItemStatData(itemStatData));
        }

        return itemObjData;
    }
}
