using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObj : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private bool onEnablePlay = true;

    private void OnEnable()
    {
        if(onEnablePlay)
            PlaySE();
    }

    public virtual void PlaySE()
    {
        SoundManager soundManager = SoundManager.instance;
        if (soundManager != null)
        {
            soundManager.PlaySE(clip);
        }
    }
}
