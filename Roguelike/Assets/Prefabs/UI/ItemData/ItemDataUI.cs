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
    /// : ����������ǥ��â ��Ȱ��/Ȱ��
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
    /// : pItemObjData������� ������ �ɷ�ġ������ ��ȯ
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
    /// : pItemObjData������� ����������ǥ��
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateItemData(ItemObjData pItemObjData, int pIdx)
    {
        CharacterManager characterManager = CharacterManager.instance;

        nowItem = pItemObjData;
        nowItemIdx = pIdx;

        itemImg.sprite = pItemObjData.itemData.itemSprite_UI;                   //������ �̹��� ǥ��
        nameText.text = LanguageManager.GetText(pItemObjData.itemData.nameKey); //������ �̸� ǥ��

        ItemType itemType = ItemManager.GetItemType(pItemObjData);

        //���� ���� ������ �����´�.
        ItemObjData nowWeapon = characterManager.NowWeapon();
        Item nowWeaponItem = nowWeapon.itemData.item;
        bool cantTakeOffWeapon = ItemManager.CantTakeOff(nowWeaponItem);        // �ش� ��� ���� �� �ִ��� �˻�

        if (itemType == ItemType.Etc)
        {
            //��Ÿ �������̴�. ������ �����⸸ Ȱ��ȭ�Ѵ�.
            equipBtn.SetActive(false);
            takeOffBtn.SetActive(false);
            dropBtn.SetActive(true);
        }
        else
        {
            //���� ���ų� ���� �����Ƿ� �ش� UI�� ǥ�����ش�.
            //equipBox : ��� ���� ���� ���� UI
            if (pItemObjData.equip)
            {
                //���� �������� ��� ���� ó��
                if (itemType == ItemType.Weapon)
                {
                    //�������� ����� ���ų� ���°��� ǥ�������� �ʴ´�.
                    //�������� ������ �� ���¿��� �Ǳ� ����
                    equipBtn.SetActive(false);
                    takeOffBtn.SetActive(false);
                    dropBtn.SetActive(false);
                }
                else if (itemType == ItemType.Accessary)
                {
                    //�������� �Ǽ��縮�� ���ų� ���°��� ǥ�� �ص��ȴ�.
                    //�Ǽ��縮 ���� ���� ���¿��� �Ǳ� ����
                    equipBtn.SetActive(false);

                    bool cantTakeOff = ItemManager.CantTakeOff(nowItem.itemData.item);
                    if (cantTakeOff)
                    {
                        //������ ���� ����.
                        takeOffBtn.SetActive(false);
                    }
                    else
                        takeOffBtn.SetActive(true);

                    dropBtn.SetActive(false);
                }
            }
            else
            {
                //���������� ���� ����� ���ų� ���°��� ǥ�����ش�.
                if(itemType == ItemType.Weapon)
                {
                    //��������̰�
                    if (cantTakeOffWeapon)
                    {
                        //���� ������ ���� ���� ��� �����ִٸ�
                        equipBtn.SetActive(false);
                    }
                    else
                        equipBtn.SetActive(true);
                }
                else if (itemType == ItemType.Accessary)
                {
                    //�Ǽ��縮 ����̰�
                    bool fullSlot = false;
                    List<ItemObjData> nowAccessary = characterManager.NowAccessaryList();
                    int accessaryCount = nowAccessary.Count;
                    if (accessaryCount >= CharacterManager.MAX_ACCESSARY_SLOT)
                    {
                        //������ ��á��.
                        fullSlot = true;
                    }

                    if (fullSlot)
                    {
                        //���� �Ǽ��縮 ������ ��á��.
                        equipBtn.SetActive(false);
                    }
                    else
                    {
                        //���Կ� ���� ���� ������ �����ִ�.
                        equipBtn.SetActive(true);
                    }
                }

                takeOffBtn.SetActive(false);
                dropBtn.SetActive(true);
            }
        }

        if (itemType == ItemType.Weapon)
        {
            //���� ���� ������ ����.
            explainBox.SetActive(false);

        }
        else
        {
            //������ ���� ������ �ִ�.
            explainBox.SetActive(true);
        }

        if(pItemObjData.itemData.itemStatDatas.Count == 0 || itemType != ItemType.Weapon)
        {
            //ǥ���� ������ ���ų� ���Ⱑ �ƴϸ�
            //�������� ��Ȱ��ȭ
            statBox.SetActive(false);
        }
        else
        {
            //�ٸ� ����
            //�������� Ȱ��ȭ
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
    /// : nowItem�� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void EquipItem()
    {
        CharacterManager characterManager = CharacterManager.instance;
        ItemType itemType = ItemManager.GetItemType(nowItem);

        if(itemType == ItemType.Weapon)
        {
            if (characterManager.ChangeItem(ref nowItem))
            {
                ActItemData(false);

                TotalUI totalUI = TotalUI.instance;
                totalUI.ActInventory(true);
            }

        }
        else if (itemType == ItemType.Accessary)
        {
            //�Ǽ��縮 ����̰�
            bool fullSlot = false;
            List<ItemObjData> nowAccessary = characterManager.NowAccessaryList();
            int accessaryCount = nowAccessary.Count;
            if (accessaryCount >= CharacterManager.MAX_ACCESSARY_SLOT)
            {
                //������ ��á��.
                fullSlot = true;
                return;
            }

            if(fullSlot == false)
            {
                nowItem.equip = true;

                ActItemData(false);

                TotalUI totalUI = TotalUI.instance;
                totalUI.ActInventory(true);
                totalUI.UpdateHp();
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem�� ���´�.
    ////////////////////////////////////////////////////////////////////////////////
    public void TakeOffItem()
    {
        CharacterManager characterManager = CharacterManager.instance;
        if (characterManager.TakeOffItem(ref nowItem))
        { 
            ActItemData(false);

            TotalUI totalUI = TotalUI.instance;
            totalUI.ActInventory(true);
            totalUI.UpdateHp();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem�� ������.
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
