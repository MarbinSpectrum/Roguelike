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

        ItemType itemType = ItemManager.GetItemType(item);

        if (itemType != ItemType.Weapon && itemObjData.itemData.stockItem)
        {
            //������ �Ǵ� �������� ������ ���������� ������ ������ ����
            itemObjData.count = Random.Range(
                itemObjData.itemData.valueMinMax.x,
                itemObjData.itemData.valueMinMax.y);
        }
        else
        {
            //�ƴ϶�� ������ �Ѱ���
            itemObjData.count = 1;
        }

        if(itemType == ItemType.Weapon)
        {
            //���������� ����
            foreach(ItemStatData itemStatData in itemStatDatas)
                itemObjData.itemStats.Add(new ItemStatData(itemStatData));

            //���� ������
            foreach (ItemStatData itemStatData in itemObjData.itemStats)
                itemStatData.GetValue();
        }

        return itemObjData;
    }
}
