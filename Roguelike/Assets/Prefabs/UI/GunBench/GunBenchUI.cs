using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 총기 선반 UI를 처리하는 곳입니다.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchUI : SerializedMonoBehaviour
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
    private GunBenchShowGunImg showGun;
    [SerializeField]
    private GunBenchStatList statList;
    [SerializeField]
    private List<GunUpgradeSlot> gunUpgradeSlots;

    [SerializeField]
    private List<ItemSlot> itemSlot;
    [SerializeField]
    private List<GameObject> selectSlot;
    [SerializeField]
    private List<TextMeshProUGUI> equipItem;
    [SerializeField]
    private TextMeshProUGUI metalCntText;

    [SerializeField]
    private GameObject dismantleBtn;
    [SerializeField]
    private GameObject dismantleUI;
    [SerializeField]
    private TextMeshProUGUI dismantle_ui_text;
    [SerializeField]
    private TextMeshProUGUI dismantle_yes;
    [SerializeField]
    private TextMeshProUGUI dismantle_no;

    private List<ItemStat> upgradeList = new List<ItemStat>();
    private int selectWeapon = 0;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템정보표시창 비활성/활성
    ////////////////////////////////////////////////////////////////////////////////
    public void ActUI()
    {
        TotalUI totalUI = TotalUI.instance;
        IsRun = !isRun;
        ActUI(isRun);
    }
    public void ActUI(bool pState)
    {
        isRun = pState;
        TotalUI totalUI = TotalUI.instance;
        totalUI.ActKeyPad(!IsRun);

        uiObj.SetActive(pState);
        if(pState)
        {
            OpenUI();
        }
    }

    private void OpenUI()
    {
        UpdateUI(0);
    }

    public void UpdateUI(int pSelect)
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        List<ItemObjData> weaponList = 
            inventoryManager.GetItemList(ItemType.Weapon);

        if(pSelect >= weaponList.Count)
        {
            UpdateUI(weaponList.Count - 1);
            return;
        }
        selectWeapon = pSelect;

        //선택한 슬롯이 어떤것인지 표시해준다.
        foreach (GameObject select in selectSlot)
            select.SetActive(false);
        selectSlot[selectWeapon].SetActive(true);

        //슬롯에 플레이어 아이템을 표시해준다.
        for (int i = 0; i < itemSlot.Count; i++)
        {
            Sprite weaponSpr = null;
            bool equipCheck = false;
            if(weaponList.Count > i)
            {
                weaponSpr = weaponList[i].itemData.itemSprite_UI;
                equipCheck = weaponList[i].equip;
            }
            itemSlot[i].slotSprite = weaponSpr;
            equipItem[i].gameObject.SetActive(equipCheck);
            equipItem[i].text = LanguageManager.GetText("EQUIP_NOW");
        }

        ItemObjData nowWeapon = weaponList[selectWeapon];

        showGun.ShowGun(nowWeapon);

        statList.UpdateUI(nowWeapon.itemStats);

        UpdateUpgradeSlot(nowWeapon);
        uint metalCnt = (uint)InventoryManager.instance.GetItemCnt(Item.ScrapMetal);
        metalCntText.text = metalCnt.ToString();

        dismantleUI.SetActive(false);
        if(nowWeapon.equip)
        {
            //장비한 장비는 분해못하므로 분해 버튼 비활성화
            dismantleBtn.SetActive(false);
        }
        else
        {
            //나머지 경우는 분해버튼 활성화
            dismantleBtn.SetActive(true);
        }
    }

    private void UpdateUpgradeSlot(ItemObjData pItemObjData)
    {
        upgradeList.Clear();
        uint metalCnt = (uint)InventoryManager.instance.GetItemCnt(Item.ScrapMetal);
        int idx = 0;
        int canUpgradePow = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradePow);
        if(canUpgradePow > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradePow), metalCnt, 
                LanguageManager.GetText("UPGRADE_POW_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            upgradeList.Add(ItemStat.CanUpgradePow);
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        int canUpgradeBalance = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeBalance);
        if (canUpgradeBalance > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradeBalance), metalCnt,
                LanguageManager.GetText("UPGRADE_BALANCE_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            upgradeList.Add(ItemStat.CanUpgradeBalance);
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        int canUpgradeCriDmg = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeCriDmg);
        if (canUpgradeCriDmg > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradeCriDmg), metalCnt,
                LanguageManager.GetText("UPGRADE_CRI_DMG_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            upgradeList.Add(ItemStat.CanUpgradeCriDmg);
            idx++;
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        int canUpgradeCriRate = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeCriRate);
        if (canUpgradeCriRate > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradeCriRate), metalCnt,
                LanguageManager.GetText("UPGRADE_CRI_RATE_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            upgradeList.Add(ItemStat.CanUpgradeCriRate);
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        for(; idx < gunUpgradeSlots.Count; idx++)
        {
            gunUpgradeSlots[idx].gameObject.SetActive(false);
        }
    }

    public void UpgradeBtn(int idx)
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        List<ItemObjData> weaponList = inventoryManager.GetItemList(ItemType.Weapon);
        int stat = ItemManager.GetTotalStatValue(weaponList[selectWeapon], upgradeList[idx]);
        int cost = stat * 50;
        uint has = (uint)inventoryManager.GetItemCnt(Item.ScrapMetal);
        if(cost <= has)
        {
            inventoryManager.RemoveItem(Item.ScrapMetal, cost);
            ItemManager.AddTotalStatValue(weaponList[selectWeapon], upgradeList[idx], 1);
            ItemManager.AddTotalStatValue(weaponList[selectWeapon],ItemStat.IsUpgrade, 1);
            UpgradeItem(weaponList[selectWeapon], upgradeList[idx]);
        }
        UpdateUI(selectWeapon);
    }

    private void UpgradeItem(ItemObjData pItemObjData,ItemStat pItemStat)
    {
        switch(pItemStat)
        {
            case ItemStat.CanUpgradePow:
                {
                    ItemManager.AddTotalStatValue(pItemObjData, ItemStat.Pow, 5);
                    return;
                }
            case ItemStat.CanUpgradeBalance:
                {
                    ItemManager.AddTotalStatValue(pItemObjData, ItemStat.Balance, 2);
                    return;
                }
            case ItemStat.CanUpgradeCriDmg:
                {
                    ItemManager.AddTotalStatValue(pItemObjData, ItemStat.CriDmg, 10);
                    return;
                }
            case ItemStat.CanUpgradeCriRate:
                {
                    ItemManager.AddTotalStatValue(pItemObjData, ItemStat.CriRate, 2);
                    return;
                }
        }    
    }

    public void DismantleUI()
    {
        if(dismantleUI.activeSelf)
        {
            dismantleUI.SetActive(false);
            return;
        }

        dismantle_ui_text.text = LanguageManager.GetText("GUN_BENCH_UI_TEXT_0");
        dismantle_yes.text = LanguageManager.GetText("YES");
        dismantle_no.text = LanguageManager.GetText("NO");

        InventoryManager inventoryManager = InventoryManager.instance;
        List<ItemObjData> weaponList =
           inventoryManager.GetItemList(ItemType.Weapon);

        ItemObjData nowWeapon = weaponList[selectWeapon];
        if (ItemManager.IsUpgradeItem(nowWeapon))
        {
            dismantleUI.SetActive(true);
        }
        else
        {
            ActDismantle();
        }
    }

    public void ActDismantle()
    {
        InventoryManager inventoryManager = InventoryManager.instance;

        List<ItemObjData> weaponList =
        inventoryManager.GetItemList(ItemType.Weapon);

        ItemObjData nowWeapon = weaponList[selectWeapon];
        int upgradeCnt = ItemManager.GetTotalStatValue(nowWeapon, ItemStat.IsUpgrade);
        int getMetal = 50 + upgradeCnt * 25;
        inventoryManager.RemoveItem(ItemType.Weapon, selectWeapon);

        ItemManager itemManager = ItemManager.instance;
        ItemObjData metalObj = itemManager.CreateItemObjData(Item.ScrapMetal);
        metalObj.count = getMetal;

        inventoryManager.AddItem(metalObj);

        UpdateUI(selectWeapon);
    }
}
