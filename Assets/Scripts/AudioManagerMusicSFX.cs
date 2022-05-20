using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMusicSFX : MonoBehaviour
{
    public static AudioManagerMusicSFX instance;

    public AudioSource bgm;

    public AudioSource[] soundEffects;

    public void Awake()
    {
        instance = this;
    }

    public void PlayBGM()
    {
       // if (bgm.isPlaying) return;
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
        //Destroy(gameObject);
    }

    public void PlaySFX(int SFXNumber)
    {
        soundEffects[SFXNumber].Stop();
        soundEffects[SFXNumber].Play();
    }

    public void StopSFX(int SFXNumber)
    {
        soundEffects[SFXNumber].Stop();
    }
}
