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

    [Space(40)]

    [SerializeField]
    private GameObject statBox;
    [SerializeField]
    private GameObject explainBox;

    [Space(40)]

    [SerializeField]
    private GameObject equipBtn;
    [SerializeField]
    private GameObject dropBtn;
    [SerializeField]
    private GameObject takeOffBtn;
    [SerializeField]
    private GameObject useBtn;

    [Space(40)]

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI explainText;
    [SerializeField]
    private TextMeshProUGUI statText;
    [SerializeField]
    private TextMeshProUGUI equipText;
    [SerializeField]
    private TextMeshProUGUI dropText;
    [SerializeField]
    private TextMeshProUGUI takeOffText;
    [SerializeField]
    private TextMeshProUGUI useText;

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
    /// : pItemObjData기반으로 아이템정보표시
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateItemData(ItemObjData pItemObjData, int pIdx)
    {
        InventoryManager inventoryManager = InventoryManager.instance;

        nowItem = pItemObjData;
        nowItemIdx = pIdx;

        itemImg.sprite = pItemObjData.itemData.itemSprite_UI;                   //아이템 이미지 표시
        nameText.text = LanguageManager.GetText(pItemObjData.itemData.nameKey); //아이템 이름 표시

        ItemType itemType = ItemManager.GetItemType(pItemObjData);

        //무기를 벗을 수 있는지 검사하기 위해서
        //현재 무기 정보를 가져온다.
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        Item nowWeaponItem = nowWeapon.itemData.item;
        bool cantTakeOffWeapon = ItemManager.CantTakeOff(nowWeapon);  

        if (itemType == ItemType.Etc)
        {
            //기타 아이템이다. 아이템 버리기만 활성화한다.
            equipBtn.SetActive(false);
            takeOffBtn.SetActive(false);
            dropBtn.SetActive(true);

            bool isUseItem = ItemManager.IsUseItem(nowItem.itemData.item);
            if(isUseItem)
            {
                useBtn.SetActive(true);
            }
            else
            {
                useBtn.SetActive(false);
            }
        }
        else
        {
            //장비는 벗거나 낄수 있으므로 해당 UI를 표시해준다.
            //equipBox : 장비 벗기 끼기 관련 UI
            if (pItemObjData.equip)
            {
                //현재 장착중인 장비에 대한 처리
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

                    bool cantTakeOff = ItemManager.CantTakeOff(nowItem);
                    if (cantTakeOff)
                    {
                        //벗을수 없는 장비다.
                        takeOffBtn.SetActive(false);
                    }
                    else
                        takeOffBtn.SetActive(true);

                    dropBtn.SetActive(false);
                }
            }
            else
            {
                //장착중이지 않은 장비의 벗거나 끼는것을 표시해준다.
                if(itemType == ItemType.Weapon)
                {
                    //무기장비이고
                    if (cantTakeOffWeapon)
                    {
                        //현재 벗을수 없는 무기 장비를 끼고있다면
                        equipBtn.SetActive(false);
                    }
                    else
                        equipBtn.SetActive(true);
                }
                else if (itemType == ItemType.Accessary)
                {
                    //악세사리 장비이고
                    bool fullSlot = false;
                    List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();
                    int accessaryCount = nowAccessary.Count;
                    if (accessaryCount >= CharacterManager.MAX_ACCESSARY_SLOT)
                    {
                        //슬롯이 꽉찼다.
                        fullSlot = true;
                    }

                    if (fullSlot)
                    {
                        //현재 악세사리 슬롯이 꽉찼다.
                        equipBtn.SetActive(false);
                    }
                    else
                    {
                        //슬롯에 아직 여유 공간이 남아있다.
                        equipBtn.SetActive(true);
                    }
                }

                takeOffBtn.SetActive(false);
                dropBtn.SetActive(true);
            }
            useBtn.SetActive(false);
        }

        if (itemType == ItemType.Weapon)
        {
            //무기 장비는 설명이 없다.
            explainBox.SetActive(false);
            int upgradeCnt = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.IsUpgrade);
            if(upgradeCnt > 0)
            {
                nameText.text += "(+" + upgradeCnt + ")";
            }
        }
        else
        {
            //나머지 경우는 설명이 있다.
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

        explainText.text = ItemManager.ItemExplainDataStr(pItemObjData);

        string statStr = ItemManager.ItemStatDataStr(pItemObjData);
        statText.text = statStr;

        equipText.text = LanguageManager.GetText("EQUIP");
        dropText.text = LanguageManager.GetText("DROP");
        takeOffText.text = LanguageManager.GetText("TAKE_OFF");
        useText.text = LanguageManager.GetText("USE");
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 장착한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void EquipItem()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemType itemType = ItemManager.GetItemType(nowItem);

        if(itemType == ItemType.Weapon)
        {
            if (inventoryManager.ChangeItem(ref nowItem))
            {
                ActItemData(false);

                TotalUI totalUI = TotalUI.instance;
                totalUI.ActInventory(true);
                totalUI.UpdateHp();
                totalUI.UpdateShield();
            }

        }
        else if (itemType == ItemType.Accessary)
        {
            //악세사리 장비이고
            bool fullSlot = false;
            List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();
            int accessaryCount = nowAccessary.Count;
            if (accessaryCount >= CharacterManager.MAX_ACCESSARY_SLOT)
            {
                //슬롯이 꽉찼다.
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
                totalUI.UpdateShield();
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 벗는다.
    ////////////////////////////////////////////////////////////////////////////////
    public void TakeOffItem()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        if (inventoryManager.TakeOffItem(ref nowItem))
        { 
            ActItemData(false);

            TotalUI totalUI = TotalUI.instance;
            totalUI.ActInventory(true);
            totalUI.UpdateHp();
            totalUI.UpdateShield();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 버린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void DropItem()
    {
        if (nowItem == null)
            return;
        ItemType itemType = ItemManager.GetItemType(nowItem);

        InventoryManager inventoryManager = InventoryManager.instance;
        inventoryManager.RemoveItem(itemType, nowItemIdx);

        ActItemData(false);

        TotalUI totalUI = TotalUI.instance;
        totalUI.ActInventory(true);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : nowItem을 사용한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UseItem()
    {
        if (nowItem == null)
            return;

        ItemType itemType = ItemManager.GetItemType(nowItem);
        if (itemType != ItemType.Etc)
            return;

        InventoryManager inventoryManager = InventoryManager.instance;
        inventoryManager.UseItem(nowItemIdx);

        ActItemData(false);
        TotalUI totalUI = TotalUI.instance;

        totalUI.ActInventory(true);

    }
}
