using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : ü�¹� ǥ��
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class HpBar : SerializedMonoBehaviour
{
    [Title("Hp Data")]
    #region[private int maxHp]
    private int MaxHp;
    [ShowInInspector,PropertyOrder(-2)]
    public int maxHp
    {
        get { return MaxHp; }
        set
        {
            MaxHp = value;
            UpdateHp();
        }
    }
    #endregion
    #region[private int Hp]
    private int Hp;
    [ShowInInspector, PropertyOrder(-1)]
    public int hp
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
    /// : ü�¹� ������Ʈ
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
}
