using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 사운드를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class SoundManager : DontDestroySingleton<SoundManager>
{
    [SerializeField]
    private AudioSource seSource;
    private List<AudioSource> seList = new List<AudioSource>();
    [SerializeField]
    private AudioSource bgmSource;

    private float SeVolume;
    [SerializeField, PropertyOrder(-2),PropertyRange(0,1)]
    public float seVolume
    {
        get
        {
            return SeVolume;
        }
        set
        {
            SeVolume = value;
            if (instance != null)
                seSource.volume = SeVolume;
        }
    }

    private float BgmVolume;
    [SerializeField, PropertyOrder(-1), PropertyRange(0, 1)]
    public float bgmVolume
    {
        get
        {
            return BgmVolume;
        }
        set
        {
            BgmVolume = value;
            if (instance != null)
                bgmSource.volume = BgmVolume;
        }
    }

    public void PlaySE(AudioClip pAudioClipe)
    {
        AudioSource audioSource = null;
        foreach(AudioSource audio in seList)
        {
            if (audio.isPlaying)
                continue;
            audioSource = audio;
        }

        if(audioSource == null)
        {
            audioSource = Instantiate(seSource);
            audioSource.transform.parent = transform;
            seList.Add(audioSource);
        }

        if (audioSource)
        {
            audioSource.clip = pAudioClipe;
            audioSource.Play();
        }
        
    }

    public void PlayBGM(AudioClip pAudioClipe)
    {
        if (bgmSource)
        {
            bgmSource.clip = pAudioClipe;
            bgmSource.Play();
        }
    }

    public void MuteBGM(bool pState)
    {
        bgmSource.mute = pState;
    }
}
