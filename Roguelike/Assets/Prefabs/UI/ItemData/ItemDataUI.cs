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
    private GameObject statBox;
    [SerializeField]
    private GameObject explainBox;
    [SerializeField]
    private GameObject equipBtn;
    [SerializeField]
    private GameObject dropBtn;
    [SerializeField]
    private GameObject takeOffBtn;

    [SerializeField]
    private TextMeshProUGUI equipText;
    [SerializeField]
    private TextMeshProUGUI dropText;
    [SerializeField]
    private TextMeshProUGUI takeOffText;

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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData기반으로 아이템 능력치정보를 반환
    ////////////////////////////////////////////////////////////////////////////////
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData기반으로 아이템정보표시
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateItemData(ItemObjData pItemObjData, int pIdx)
    {
        nowItem = pItemObjData;
        nowItemIdx = pIdx;

        itemImg.sprite = pItemObjData.itemData.itemSprite_UI;                   //아이템 이미지 표시
        nameText.text = LanguageManager.GetText(pItemObjData.itemData.nameKey); //아이템 이름 표시

        ItemType itemType = ItemManager.GetItemType(pItemObjData);
        if (itemType == ItemType.Etc)
        {
            //기타 아이템이다. 아이템 버리기만 활성화한다.
            equipBtn.SetActive(false);
            takeOffBtn.SetActive(false);
            dropBtn.SetActive(true);
        }
        else
        {
            //장비는 벗거나 낄수 있으므로 해당 UI를 표시해준다.
            //equipBox : 장비 벗기 끼기 관련 UI
            if (pItemObjData.equip)
            {
                if (itemType == ItemType.Weapon)
                {
                    //장착중인 무기는 벗거나 끼는것을 표시해주지 않는다.
                    //무기장비는 무조건 낀 상태여야 되기 때문
                    equipBtn.SetActive(false);
                    takeOffBtn.SetActive(false);
                    dropBtn.SetActive(false);
                }
                else if (itemType == ItemType.Accessary)
                {
                    //장착중인 악세사리는 벗거나 끼는것을 표시 해도된다.
                    //악세사리 장비는 벗은 상태여도 되기 때문
                    equipBtn.SetActive(false);
                    takeOffBtn.SetActive(true);
                    dropBtn.SetActive(false);
                }
            }
            else
            {
                //장착중이지 않은 장비는 벗거나 끼는것을 표시해준다.
                equipBtn.SetActive(true);
                takeOffBtn.SetActive(false);
                dropBtn.SetActive(true);
            }
        }

        if (itemType == ItemType.Weapon)
        {
            //무기 장비는 설명이 없다.
            explainBox.SetActive(false);

        }
        else
        {
            //나머지는 설명이 없다.
            explainBox.SetActive(true);
        }

        if(pItemObjData.itemData.itemStatDatas.Count == 0 || itemType != ItemType.Weapon)
        {
            //표시할 스텟이 없거나 무기가 아니면
            //스텟정보 비활성화
            statBox.SetActive(false);
        }
        else
        {
            //다른 경우는
            //스텟정보 활성화
            statBox.SetActive(true);
        }

        explainText.text = LanguageManager.GetText(pItemObjData.itemData.explainKey);

        string statStr = ItemStatDataStr(pItemObjData);
        statText.text = statStr;

        equipText.text = LanguageManager.GetText("EQUIP");
        dropText.text = LanguageManager.GetText("DROP");
        takeOffText.text = LanguageManager.GetText("TAKE_OFF");
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 장착한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void EquipItem()
    {
        CharacterManager characterManager = CharacterManager.instance;
        if(characterManager.ChangeItem(ref nowItem))
        {
            ActItemData(false);

            TotalUI totalUI = TotalUI.instance;
            totalUI.ActInventory(true);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 벗는다.
    ////////////////////////////////////////////////////////////////////////////////
    public void TakeOffItem()
    {
        CharacterManager characterManager = CharacterManager.instance;
        if (characterManager.TakeOffItem(ref nowItem))
        { 
            ActItemData(false);

            TotalUI totalUI = TotalUI.instance;
            totalUI.ActInventory(true);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 버린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void DropItem()
    {
        if (nowItem != null)
        {
            ItemType itemType = ItemManager.GetItemType(nowItem);

            TotalUI totalUI = TotalUI.instance;
            totalUI.InventoryRemoveItem(itemType, nowItemIdx);

            ActItemData(false);

            totalUI.ActInventory(true);
        }
    }
}
