using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 체력바 표시
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class ShieldBar : SerializedMonoBehaviour
{
    [Title("Shield Data")]
    #region[private uint Shield]
    private int Shield;
    [ShowInInspector, PropertyOrder(-1)]
    public int shield
    {
        get { return Shield; }
        set
        {
            Shield = value;
            UpdateShield();
        }
    }
    #endregion

    [Title("Shield Data")]
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
    private RectTransform shieldBar;
    [SerializeField]
    private RectTransform barMask;
    [SerializeField]
    private List<RectTransform> slotObj = new List<RectTransform>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : 체력바 업데이트
    ////////////////////////////////////////////////////////////////////////////////
    private void UpdateShield()
    {
        foreach (RectTransform game in slotObj)
        {
            game.gameObject.SetActive(false);
        }

        float maskWidth = 0;
        float hpWidth = 0;
        Vector2 pos = Vector2.zero;

        for (int i = 0; i < shield; i++)
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

            if (i < shield)
            {
                if (i == 0)
                    hpWidth += startAddWidth;
                else
                    hpWidth += addWidth;
            }
        }

        barMask.sizeDelta = new Vector2(maskWidth, imageHeight);
        shieldBar.sizeDelta = new Vector2(hpWidth, imageHeight);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 체력 업데이트
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateShield(int pNowShield)
    {
         shield = pNowShield;
    }
}
