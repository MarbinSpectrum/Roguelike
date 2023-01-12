using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 경험치바 표시
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class ExpBar : SerializedMonoBehaviour
{
    [SerializeField]
    private Image expBar;
    #region[private uint maxHp]
    private uint MaxExp;
    [ShowInInspector, PropertyOrder(-2)]
    public uint maxExp
    {
        get { return MaxExp; }
        set
        {
            MaxExp = value;
            UpdateExp();
        }
    }
    #endregion
    #region[private uint Exp]
    private uint Exp;
    [ShowInInspector, PropertyOrder(-1)]
    public uint exp
    {
        get { return Exp; }
        set
        {
            Exp = value;
            UpdateExp();
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    /// : 경험치바 업데이트
    ////////////////////////////////////////////////////////////////////////////////
    private void UpdateExp()
    {
        float amount = (float)Exp / (float)MaxExp;
        expBar.fillAmount = amount;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 경험치 업데이트
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateExp(uint pMaxExp, uint pNowExp)
    {
        maxExp = pMaxExp;
        exp = pNowExp;
    }
}
