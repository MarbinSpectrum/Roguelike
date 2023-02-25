using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : 아이템의 이름과 이미지를 보여준다.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchShowGunImg : MonoBehaviour
{
    [SerializeField]
    private Image gunImg;

    [SerializeField]
    private TextMeshProUGUI gunName;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData에서 아이템이미지와 이름을 가져온다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowGun(ItemObjData pItemObjData)
    {
        gunName.text = LanguageManager.GetText(pItemObjData.itemData.nameKey);
        gunImg.sprite = pItemObjData.itemData.itemSprite_UI;
    }
}
