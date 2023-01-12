using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : ����ġ�� ǥ��
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
    /// : ����ġ�� ������Ʈ
    ////////////////////////////////////////////////////////////////////////////////
    private void UpdateExp()
    {
        float amount = (float)Exp / (float)MaxExp;
        expBar.fillAmount = amount;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ġ ������Ʈ
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateExp(uint pMaxExp, uint pNowExp)
    {
        maxExp = pMaxExp;
        exp = pNowExp;
    }
}
