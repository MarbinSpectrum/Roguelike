using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : DontDestroySingleton<UIEffectManager>
{
    [SerializeField] private    Canvas          canvas;
    [SerializeField] public     DamageEffect    damageEffect
    {
        get;
        private set;
    }
    [SerializeField] public     GetGoldEffect   getGoldEffect
    {
        get;
        private set;
    }

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
}
