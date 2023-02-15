using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GurdianEffect_Prefab : MonoBehaviour
{
    public Animation animation;

    public void EnQuque()
    {
        GurdianEffect.instance.Enqueue(this);
    }
}
