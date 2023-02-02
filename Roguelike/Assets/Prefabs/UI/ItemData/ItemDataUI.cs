using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDataUI : MonoBehaviour
{
    #region[private bool isRun]
    private bool IsRun;
    public bool isRun
    {
        get
        {
            return IsRun;
        }
        set
        {
            IsRun = value;
        }

    }
    #endregion

    [SerializeField]
    private GameObject uiObj;

    [SerializeField]
    private Image itemImg;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI explainText;
    [SerializeField]
    private TextMeshProUGUI statText;

    [SerializeField]
    private GameObject explainBox;
    [SerializeField]
    private GameObject equipBox;
    [SerializeField]
    private TextMeshProUGUI equipText;
    [SerializeField]
    private TextMeshProUGUI dropText;

    private ItemObjData nowItem;
    private int nowItemIdx;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템정보표시창 비활성/활성
    ////////////////////////////////////////////////////////////////////////////////
    public void ActItemData()
    {
        isRun = !isRun;
        uiObj.SetActive(isRun);
    }
    public void ActItemData(bool pState)
    {
        isRun = pState;
        uiObj.SetActive(pState);
    }

    public string ItemStatDataStr(ItemObjData pItemObjData)
    {
        string str = "";
        List<ItemStatData> itemStatDatas = pItemObjData.itemStats;
        foreach(ItemStatData itemStatData in itemStatDatas)
        {
            ItemStat itemStat = itemStatData.itemStat;
            int statValue = itemStatData.GetValue();
            switch(itemStat)
            {
                case ItemStat.Pow:
                    str += string.Format("{0} +{1}", LanguageManager.GetText("ATK"), statValue);
                    break;
                case ItemStat.Balance:
                    str += string.Format("{0} +{1}", LanguageManager.GetText("BALANCE"), statValue);
                    break;
                case ItemStat.CriPer:
                    str += string.Format("{0} +{1}%", LanguageManager.GetText("CRI_RATE"), statValue);
                    break;
                case ItemStat.CriDmg:
                    str += string.Format("{0} +{1}%", LanguageManager.GetText("CRI_DMG"), statValue);
                    break;
                case ItemStat.Hp:
                    str += string.Format("{0} +{1}", LanguageManager.GetText("HP"), statValue);
                    break;
                case ItemStat.AddExp:
                    str += string.Format("{0} +{1}%", LanguageManager.GetText("ADD_EXP"), statValue);
                    break;
                case ItemStat.AddGold:
                    str += string.Format("{0} +{1}%", LanguageManager.GetText("ADD_GOLD"), statValue);
                    break;
            }
            str += "\n";
        }
        return str;
    }

    public void UpdateItemData(ItemObjData pItemObjData, int pIdx)
    {
        nowItem = pItemObjData;
        nowItemIdx = pIdx;

        itemImg.sprite = pItemObjData.itemData.itemSprite_UI;
        nameText.text = LanguageManager.GetText(pItemObjData.itemData.nameKey);
        if (ItemManager.GetItemType(pItemObjData) == ItemType.Weapon)
        {
            if (pItemObjData.equip)
                equipBox.SetActive(false);
            else
                equipBox.SetActive(true);
            explainBox.SetActive(false);
        }
        else
        {
            equipBox.SetActive(false);
            explainBox.SetActive(true);
        }
        explainText.text = LanguageManager.GetText(pItemObjData.itemData.explainKey);

        string statStr = ItemStatDataStr(pItemObjData);
        statText.text = statStr;

        equipText.text = LanguageManager.GetText("EQUIP");
        dropText.text = LanguageManager.GetText("DROP");
    }

    public void EquipItem()
    {
        if(ItemManager.GetItemType(nowItem) == ItemType.Weapon)
        {
            ItemObjData nowWeapon = CharacterManager.instance.NowWeapon();
            nowWeapon.equip = false;

            nowItem.equip = true;
            ActItemData();

            TotalUI totalUI = TotalUI.instance;
            totalUI.ActInventory(true);
        }
    }

    public void DropItem()
    {
        if (ItemManager.GetItemType(nowItem) == ItemType.Weapon)
        {
            TotalUI totalUI = TotalUI.instance;
            totalUI.InventoryRemoveItem(ItemType.Weapon, nowItemIdx);

            ActItemData();

            totalUI.ActInventory(true);
        }
    }
}
