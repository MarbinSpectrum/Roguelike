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

    private int selectWeapon = 0;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템정보표시창 비활성/활성
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Act UI", ButtonSizes.Large)]
    public void ActUI()
    {
        isRun = !isRun;
        ActUI(isRun);
    }
    public void ActUI(bool pState)
    {
        isRun = pState;
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

        selectWeapon = pSelect;

        //선택한 슬롯이 어떤것인지 표시해준다.
        foreach (GameObject select in selectSlot)
            select.SetActive(false);
        selectSlot[selectWeapon].SetActive(true);

        //슬롯에 플레이어 아이템을 표시해준다.
        List<ItemObjData> weaponList = 
            inventoryManager.GetItemList(ItemType.Weapon);
        for(int i = 0; i < itemSlot.Count; i++)
        {
            Sprite weaponSpr = null;
            if(weaponList.Count > i)
            {
                weaponSpr = weaponList[i].itemData.itemSprite_UI;
            }
            itemSlot[i].slotSprite = weaponSpr;
        }

        showGun.ShowGun(weaponList[selectWeapon]);

        statList.UpdateUI(weaponList[selectWeapon].itemStats);

        UpdateUpgradeSlot(weaponList[selectWeapon]);
    }

    private void UpdateUpgradeSlot(ItemObjData pItemObjData)
    {
        int idx = 0;
        int canUpgradePow = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradePow);
        if(canUpgradePow > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradePow), 0, 
                LanguageManager.GetText("UPGRADE_POW_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        int canUpgradeBalance = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeBalance);
        if (canUpgradeBalance > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradeBalance), 0,
                LanguageManager.GetText("UPGRADE_BALANCE_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        int canUpgradeCriDmg = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeCriDmg);
        if (canUpgradeCriDmg > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradeCriDmg), 0,
                LanguageManager.GetText("UPGRADE_CRI_DMG_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        int canUpgradeCriRate = ItemManager.GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeCriRate);
        if (canUpgradeCriRate > 0)
        {
            gunUpgradeSlots[idx].UpdateUI((uint)(50 * canUpgradeCriRate), 0,
                LanguageManager.GetText("UPGRADE_CRI_RATE_EXPLAIN"));
            gunUpgradeSlots[idx].gameObject.SetActive(true);
            idx++;
            if (gunUpgradeSlots.Count <= idx)
                return;
        }

        for(; idx < gunUpgradeSlots.Count; idx++)
        {
            gunUpgradeSlots[idx].gameObject.SetActive(false);
        }
    }
}
