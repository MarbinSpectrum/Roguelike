using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BulletEffect : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<Item, GameObject> animations;

    public void OffAni()
    {
        foreach (KeyValuePair<Item, GameObject> animationPair in animations)
            animationPair.Value.SetActive(false);
    }

    public void RunBulletAni(Item pItem)
    {
        OffAni();
        if (animations.ContainsKey(pItem))
            animations[pItem].SetActive(true);
    }
}
