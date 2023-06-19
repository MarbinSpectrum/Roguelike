using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Explain_Text : Mgr
{
    [SerializeField]
    private TextMeshProUGUI showText;
    [SerializeField]
    private Animation showAni;

    public void ShowUI(string pString)
    {
        showAni.Play();
        showText.text = pString;
    }

    public void Update()
    {
        Vector3 textPos = characterMgr.CharactorTransPos();
        transform.position = textPos;
    }
}
