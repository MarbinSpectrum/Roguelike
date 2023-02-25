using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : 업그레이드 슬롯입니다. 업그레이드에 대한 정보를 제공합니다.
////////////////////////////////////////////////////////////////////////////////
public class GunUpgradeSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI upgradeExplain;
    [SerializeField]
    private TextMeshProUGUI numText;
    [SerializeField]
    private TextMeshProUGUI numTextRed;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 다음 정보를 바탕으로 업그레이드 슬롯을 표시합니다.
    /// : pCost : 업그레이드 비용입니다.
    /// : pHasCost : 플레이어가 현재 가지고 있는 골드를 의미합니다.
    /// : pExplainStr : 업그레이드에 대한 설명 텍스트입니다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateUI(uint pCost,uint pHasCost,string pExplainStr)
    {
        upgradeExplain.text = pExplainStr;
        if(pCost <= pHasCost)
        {
            //비용이 충분합니다.
            //보통색의 글자로 표시합니다.
            numText.enabled = true;
            numTextRed.enabled = false;
            numText.text = pCost.ToString();
        }
        else
        {
            //비용이 부족합니다.
            //빨간색 글자로 표시합니다.
            numText.enabled = false;
            numTextRed.enabled = true;
            numTextRed.text = pCost.ToString();
        }
    }
}
