using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapName : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI mapNameText;
    [SerializeField]
    private TextMeshProUGUI aniText;

    [SerializeField]
    private Animation showMapName;

    public void ShowMapName(string pStr)
    {
        mapNameText.text = pStr;
        aniText.text = pStr;
        showMapName.Play();
    }
}
