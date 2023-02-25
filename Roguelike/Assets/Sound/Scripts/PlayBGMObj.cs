using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : BGM���� ������Ʈ
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
        SoundManager.PlayBGM(clip);
    }
}
