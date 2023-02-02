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
            switch(itemStat)
            {
                case ItemStat.AddExp:
                    {
                       // str += "아이"
                    }
                    break;
            }
        }
        return str;
    }

    public void UpdateItemData(ItemObjData pItemObjData)
    {
        itemImg.sprite = pItemObjData.itemData.itemSprite_UI;
        nameText.text = LanguageManager.GetText(pItemObjData.itemData.nameKey);
        explainText.text = LanguageManager.GetText(pItemObjData.itemData.explainKey);
        statText.text = "";
    }
}
