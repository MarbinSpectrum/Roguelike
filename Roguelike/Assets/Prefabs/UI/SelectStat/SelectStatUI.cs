using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelectStatUI : MonoBehaviour
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
    private TextMeshProUGUI powText;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI balanceText;

    [Space(40)]

    [SerializeField]
    private int addPow;
    [SerializeField]
    private int addHp;
    [SerializeField]
    private int addBalance;


    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ݼ���â ��Ȱ��/Ȱ��
    ////////////////////////////////////////////////////////////////////////////////
    public void ActSelectStat()
    {
        ActSelectStat(!isRun);
    }
    public void ActSelectStat(bool pState)
    {
        TotalUI totalUI = TotalUI.instance;
        totalUI.ActKeyPad(!pState);

        isRun = pState;
        uiObj.SetActive(isRun);
        UpdateUI();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : addPow��ŭ ���ݷ� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void AddPow()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.AddPow(addPow);


        ActSelectStat(false);
        characterManager.LevelUpCheck();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : addHp��ŭ �ִ�ü�� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMaxHp()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.AddMaxHp(addHp);

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(characterManager.GetTotalMaxHp(), characterManager.nowHp);

        ActSelectStat(false);

        characterManager.LevelUpCheck();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : addBalance��ŭ �뷱�� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void AddBalance()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.AddBalance(addBalance);

        ActSelectStat(false);

        characterManager.LevelUpCheck();
    }

    private void UpdateUI()
    {
        string powStr = string.Format(LanguageManager.GetText("SELECT_STAT_POW_TEXT"), addPow);
        powText.text = powStr;

        string hpStr = string.Format(LanguageManager.GetText("SELECT_STAT_HP_TEXT"), addHp);
        hpText.text = hpStr;

        string balanceStr = string.Format(LanguageManager.GetText("SELECT_STAT_BALANCE_TEXT"), addBalance);
        balanceText.text = balanceStr;
    }
}
