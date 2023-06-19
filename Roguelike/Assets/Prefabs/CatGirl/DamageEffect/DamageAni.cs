using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageAni : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public bool isCritical;

    public void UpdateText(string pString)
    {
        textUI.text = pString;
    }

    public void AnimationEnd()
    {
        UIEffectManager uIEffectManager = UIEffectManager.instance;
        DamageEffect damageEffect = uIEffectManager.damageEffect;
        damageEffect.Enqueue(this, isCritical);
        gameObject.SetActive(false);
    }
}
