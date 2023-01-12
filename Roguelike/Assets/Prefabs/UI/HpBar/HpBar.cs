using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 체력바 표시
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class HpBar : SerializedMonoBehaviour
{
    [Title("Hp Data")]
    #region[private uint maxHp]
    private uint MaxHp;
    [ShowInInspector, PropertyOrder(-2)]
    public uint maxHp
    {
        get { return MaxHp; }
        set
        {
            MaxHp = value;
            UpdateHp();
        }
    }
    #endregion
    #region[private uint Hp]
    private uint Hp;
    [ShowInInspector, PropertyOrder(-1)]
    public uint hp
    {
        get { return Hp; }
        set
        {
            Hp = value;
            UpdateHp();
        }
    }
    #endregion

    [Title("Hp Data")]
    [SerializeField]
    private float startAddDis;
    [SerializeField]
    private float addDis;

    [Title("Mask Data")]
    [SerializeField]
    private float startAddWidth;
    [SerializeField]
    private float addWidth;

    [Title("Etc Data")]
    [SerializeField]
    private float imageHeight = 1.0591f;

    [Title("RequireData")]
    [SerializeField]
    private RectTransform hpBar;
    [SerializeField]
    private RectTransform barMask;
    [SerializeField]
    private List<RectTransform> slotObj = new List<RectTransform>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : 체력바 업데이트
    ////////////////////////////////////////////////////////////////////////////////
    private void UpdateHp()
    {
        foreach (RectTransform game in slotObj)
        {
            game.gameObject.SetActive(false);
        }

        float maskWidth = 0;
        float hpWidth = 0;
        Vector2 pos = Vector2.zero;

        for (int i = 0; i < maxHp; i++)
        {
            if (slotObj.Count <= i)
                continue;

            slotObj[i].gameObject.SetActive(true);
            slotObj[i].transform.localPosition = pos;

            if (i == 0)
                pos.x += startAddDis;
            else
                pos.x += addDis;

            if (i == 0)
                maskWidth += startAddWidth;
            else
                maskWidth += addWidth;

            if (i < hp)
            {
                if (i == 0)
                    hpWidth += startAddWidth;
                else
                    hpWidth += addWidth;
            }
        }

        barMask.sizeDelta = new Vector2(maskWidth, imageHeight);
        hpBar.sizeDelta = new Vector2(hpWidth, imageHeight);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 체력 업데이트
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateHp(uint pMaxHp, uint pNowHp)
    {
        maxHp = pMaxHp;
        hp = pNowHp;
    }
}
