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
    /// : 스텟설정창 비활성/활성
    ////////////////////////////////////////////////////////////////////////////////
    public void ActSelectStat()
    {
        ActSelectStat(!isRun);
    }
    public void ActSelectStat(bool pState)
    {
        SoundManager soundManager = SoundManager.instance;
        soundManager.MuteBGM(pState);

        TotalUI totalUI = TotalUI.instance;
        totalUI.ActKeyPad(!pState);

        isRun = pState;
        uiObj.SetActive(isRun);
        UpdateUI();

    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : addPow만큼 공격력 증가
    ////////////////////////////////////////////////////////////////////////////////
    public void AddPow()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.AddPow(addPow);


        ActSelectStat(false);
        characterManager.LevelUpCheck();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : addHp만큼 최대체력 증가
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMaxHp()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.AddMaxHp(addHp);

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp();

        ActSelectStat(false);

        characterManager.LevelUpCheck();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : addBalance만큼 밸런스 증가
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
