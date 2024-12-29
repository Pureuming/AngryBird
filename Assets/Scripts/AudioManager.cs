using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(_audioSource); // Scene이 넘어가도 파괴되지 않게 설정
    }

    public void PlayButtonAudio(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}
