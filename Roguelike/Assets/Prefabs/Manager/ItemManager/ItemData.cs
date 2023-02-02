using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 4)]
public class ItemData : ScriptableObject
{
    public Item item;
    public ItemObj itemObj;
    public Sprite itemSprite_UI;

    [ShowIf("@ItemManager.GetItemType(item) != ItemType.Weapon")]
    public bool stockItem;
    [ShowIf("@ItemManager.GetItemType(item) != ItemType.Weapon && stockItem"),MinMaxSlider(0, 100)]
    public Vector2Int valueMinMax;

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public List<ItemStatData> itemStatDatas = new List<ItemStatData>();

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public Sprite frontSprite;

    [ShowIf("@ItemManager.GetItemType(item) == ItemType.Weapon")]
    [BoxGroup("Box")]
    public Sprite sideSprite;

    [Space(100)]
    public string nameKey;
    public string explainKey;

    public ItemObjData createItemObjData()
    {
        ItemObjData itemObjData = new ItemObjData();
        itemObjData.itemData = this;

        ItemType itemType = ItemManager.GetItemType(item);

        if (itemType != ItemType.Weapon && itemObjData.itemData.stockItem)
        {
            //스톡이 되는 아이템은 랜덤한 범위내에서 아이템 갯수슬 설정
            itemObjData.count = Random.Range(
                itemObjData.itemData.valueMinMax.x,
                itemObjData.itemData.valueMinMax.y);
        }
        else
        {
            //아니라면 무조건한개임
            itemObjData.count = 1;
        }

        if(itemType == ItemType.Weapon)
        {
            //스탯정보를 대입
            foreach(ItemStatData itemStatData in itemStatDatas)
                itemObjData.itemStats.Add(new ItemStatData(itemStatData));

            //값을 정해줌
            foreach (ItemStatData itemStatData in itemObjData.itemStats)
                itemStatData.GetValue();
        }

        return itemObjData;
    }
}
