using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GlobalSingleton<AudioManager>
{
    private AudioSource _audioSource;

    private void Start()
    {
        InitAudioSource();
    }

    public void PlaySFX(AudioClip clip)
    {
        InitAudioSource();
        _audioSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        InitAudioSource();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void StopBGM()
    {
        InitAudioSource();
        _audioSource.Stop();
    }


    public void SetBGMPlayRate(float rate)
    {
        InitAudioSource();
        _audioSource.pitch = rate;
    }

    private void InitAudioSource()
    {
        if(_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }

}
