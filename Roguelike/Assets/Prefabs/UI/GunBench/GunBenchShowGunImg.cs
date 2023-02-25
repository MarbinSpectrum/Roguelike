using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : �������� �̸��� �̹����� �����ش�.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchShowGunImg : MonoBehaviour
{
    [SerializeField]
    private Image gunImg;

    [SerializeField]
    private TextMeshProUGUI gunName;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData���� �������̹����� �̸��� �����´�.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowGun(ItemObjData pItemObjData)
    {
        gunName.text = LanguageManager.GetText(pItemObjData.itemData.nameKey);
        gunImg.sprite = pItemObjData.itemData.itemSprite_UI;
    }
}
