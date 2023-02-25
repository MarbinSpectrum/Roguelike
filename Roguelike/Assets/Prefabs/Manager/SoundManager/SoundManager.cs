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

    private static float SeVolume;
    public static float seVolume
    {
        get
        {
            return SeVolume;
        }
        set
        {
            SeVolume = value;
            if (instance != null)
                instance.seSource.volume = SeVolume;
        }
    }

    private static float BgmVolume;
    public static float bgmVolume
    {
        get
        {
            return BgmVolume;
        }
        set
        {
            BgmVolume = value;
            if (instance != null)
                instance.bgmSource.volume = BgmVolume;
        }
    }

    public static void PlaySE(AudioClip pAudioClipe)
    {
        AudioSource audioSource = null;
        foreach(AudioSource audio in instance.seList)
        {
            if (audio.isPlaying)
                continue;
            audioSource = audio;
        }

        if(audioSource == null)
        {
            audioSource = Instantiate(instance.seSource);
            audioSource.transform.parent = instance.transform;
            instance.seList.Add(audioSource);
        }

        if (audioSource)
        {
            audioSource.clip = pAudioClipe;
            audioSource.Play();
        }     
    }

    public static void PlayBGM(AudioClip pAudioClipe)
    {
        AudioSource audioSource = instance.bgmSource;
        if (audioSource == null)
            return;

        audioSource.clip = pAudioClipe;
        audioSource.Play();
    }

    public static void StopBGM()
    {
        instance.bgmSource.Stop();
    }


    public static void MuteBGM(bool pState)
    {
        instance.bgmSource.mute = pState;
    }
}
