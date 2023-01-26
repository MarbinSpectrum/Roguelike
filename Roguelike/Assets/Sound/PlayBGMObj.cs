using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : BGM실행 오브젝트
////////////////////////////////////////////////////////////////////////////////
public class PlayBGMObj : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;

    private void OnEnable()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        SoundManager soundManager = SoundManager.instance;
        if (soundManager != null)
        {
            soundManager.PlayBGM(clip);
        }
        Destroy(this);
    }
}
