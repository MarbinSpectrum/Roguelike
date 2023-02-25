using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : 맵 이름 표시관련해서 처리하는 곳입니다.
////////////////////////////////////////////////////////////////////////////////
public class MapName : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI mapNameText;
    [SerializeField]
    private TextMeshProUGUI aniText;

    [SerializeField]
    private Animation showMapName;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 맵 이름을 표시합니다.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowMapName(string pStr, bool pPlayAni)
    {
        mapNameText.text = pStr;
        aniText.text = pStr;
        if (pPlayAni)
        {
            //씬 시작시 현재 맵이름을 표시합니다.
            showMapName.Play();
        }
    }
}
