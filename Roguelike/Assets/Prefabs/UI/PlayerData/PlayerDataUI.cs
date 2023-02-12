using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataUI : MonoBehaviour
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

    [Space(40)]

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI expText;

    [SerializeField]
    private TextMeshProUGUI powText;
    [SerializeField]
    private TextMeshProUGUI balanceText;
    [SerializeField]
    private TextMeshProUGUI criPerText;
    [SerializeField]
    private TextMeshProUGUI criPerDamage;

    [Space(40)]

    [SerializeField]
    private TextMeshProUGUI headerText;
    [SerializeField]
    private TextMeshProUGUI hpHeaderText;
    [SerializeField]
    private TextMeshProUGUI expHeaderText;
    [SerializeField]
    private TextMeshProUGUI powHeaderText;
    [SerializeField]
    private TextMeshProUGUI balanceHeaderText;
    [SerializeField]
    private TextMeshProUGUI critRateHeaderText;
    [SerializeField]
    private TextMeshProUGUI critDmgHeaderText;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾�����â ��Ȱ��/Ȱ��
    ////////////////////////////////////////////////////////////////////////////////
    public void ActPlayerData()
    {
        isRun = !isRun;
        uiObj.SetActive(isRun);
        UpdateUI();
    }

    public void UpdateUI()
    {
        CharacterManager characterManager = CharacterManager.instance;

        headerText.text = LanguageManager.GetText("PLAYER_DATA");
        hpHeaderText.text = LanguageManager.GetText("HP");
        expHeaderText.text = LanguageManager.GetText("EXP");
        powHeaderText.text = LanguageManager.GetText("POW");
        balanceHeaderText.text = LanguageManager.GetText("BALANCE");
        critRateHeaderText.text = LanguageManager.GetText("CRI_RATE");
        critDmgHeaderText.text = LanguageManager.GetText("CRI_DMG");

        levelText.text = characterManager.nowLevel.ToString();
        hpText.text = characterManager.nowHp.ToString() + "/" + characterManager.GetTotalMaxHp().ToString();
        float expPer = characterManager.nowExp / (float)characterManager.maxExp;
        expPer *= 100;
        expText.text = string.Format("{0:00.00}", expPer) + "%";

        SetText(ref powText, characterManager.basePow, characterManager.GetTotalPow());
        SetText(ref balanceText, characterManager.baseBalance, characterManager.GetTotalBalance());
        SetText(ref criPerText, characterManager.baseCriPer, characterManager.GetTotalCriPer(),"%");
        SetText(ref criPerDamage, characterManager.baseCriDamage, characterManager.GetTotalCriDamage(), "%");
    }

    private void SetText(ref TextMeshProUGUI pTextMeshProUGUI, float pBase, float pTotal,string pAddText = "")
    {
        float addValue = pTotal - pBase;

        if (pBase > pTotal)
            pTextMeshProUGUI.color = Color.red;
        else
            pTextMeshProUGUI.color = Color.white;

        if(addValue > 0)
        {
            string str = string.Format("{0}{1}\n<color=#999999>({2}+{3})</color>", pTotal, pAddText, pBase, addValue);
            pTextMeshProUGUI.text = str;
        }
        else
        {
            string str = string.Format("{0}{1}", pTotal, pAddText);
            pTextMeshProUGUI.text = str;
        }
    }
}
