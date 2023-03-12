using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : 상점 UI를 처리하는 곳입니다.
////////////////////////////////////////////////////////////////////////////////
public class ShopUI : SerializedMonoBehaviour
{
    public const int SHOP_SLOT_NUM = 8;

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
    private List<ItemSlot> shopItemSlots = new List<ItemSlot>();

    [SerializeField]
    private TextMeshProUGUI coinText;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아이템정보표시창 비활성/활성
    ////////////////////////////////////////////////////////////////////////////////
    public void ActUI()
    {
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
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        ShopManager shopManager = ShopManager.instance;
        List<ItemObjData> shopItem = shopManager.GetShopItemList();

        for(int i = 0; i < SHOP_SLOT_NUM; i++)
        {
            if(shopItem[i] == null)
            {
                shopItemSlots[i].item = Item.Null;
                shopItemSlots[i].slotSprite = null;
                shopItemSlots[i].slotNum = 0;
            }
            else
            {
                shopItemSlots[i].item = shopItem[i].itemData.item;
                shopItemSlots[i].slotSprite = shopItem[i].itemData.itemSprite_UI;
                shopItemSlots[i].slotNum = (uint)shopItem[i].count;
                if (shopManager.IsItemBuy(i))
                {

                }
            }
        }

        InventoryManager inventoryManager = InventoryManager.instance;
        int getCoinNum = inventoryManager.GetItemCnt(Item.Coin);
        coinText.text = getCoinNum.ToString();
    }
}
