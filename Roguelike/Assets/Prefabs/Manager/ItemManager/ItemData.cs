using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 4)]
public class ItemData : ScriptableObject
{
    public Item item;
    public ItemObj itemObj;
    public Sprite itemSprite_UI;
    public bool stockItem;

    [ShowIf("stockItem"),MinMaxSlider(0, 100)]
    public Vector2Int valueMinMax;
}
