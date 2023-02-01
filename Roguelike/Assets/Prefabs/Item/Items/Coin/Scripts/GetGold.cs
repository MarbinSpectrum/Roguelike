using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetGold : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    public void UpdateText(string pString)
    {
        textUI.text = pString;
    }

    public void AnimationEnd()
    {
        GetGoldEffect getGoldEffect = GetGoldEffect.instance;
        getGoldEffect.Enqueue(this);
        gameObject.SetActive(false);
    }
}
