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
        UIEffectManager uIEffectManager = UIEffectManager.instance;
        GetGoldEffect getGoldEffect = uIEffectManager.getGoldEffect;
        getGoldEffect.Enqueue(this);
        gameObject.SetActive(false);
    }
}
