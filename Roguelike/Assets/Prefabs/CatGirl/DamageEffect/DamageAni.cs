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
        DamageEffect damageEffect = DamageEffect.instance;
        damageEffect.Enqueue(this, isCritical);
        gameObject.SetActive(false);
    }
}
