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

    ////////////////////////////////////////////////////////////////////////////////
    /// : 인벤토리 비활성/활성
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

        levelText.text = characterManager.nowLevel.ToString();
        hpText.text = characterManager.nowHp.ToString() + "/" + characterManager.maxHp.ToString();
        float expPer = characterManager.nowExp / (float)characterManager.maxExp;
        expPer *= 100;
        expText.text = string.Format("{0:00.00}", expPer) + "%";

        powText.text = characterManager.GetTotalPow().ToString();
        balanceText.text = characterManager.GetTotalBalance().ToString();
        criPerText.text = characterManager.GetTotalCriPer().ToString() + "%";
        criPerDamage.text = characterManager.GetTotalDamage().ToString() + "%";
    }
}
