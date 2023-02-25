using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDark : FieldObjectSingleton<ScreenDark>
{
    [SerializeField]
    private Animator animator;

    private string nowAni;

    public static void AnimationState(bool state)
    {
        if (state == true && instance.nowAni != "Alpha1")
            instance.animator.SetTrigger("Alpha1");
        else if(state == false && instance.nowAni != "Alpha0")
            instance.animator.SetTrigger("Alpha0");
    }

    public void NowAni(string pStr)
    {
        nowAni = pStr;
    }
}
