using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObj : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;

    private void OnEnable()
    {
        PlaySE();
    }

    public void PlaySE()
    {
        SoundManager soundManager = SoundManager.instance;
        if (soundManager != null)
        {
            soundManager.PlaySE(clip);
        }
    }
}
