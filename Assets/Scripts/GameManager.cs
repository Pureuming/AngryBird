using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioListener AudioListener;
    private bool isMute = false;

    private void Awake()
    {
        AudioListener = GetComponent<AudioListener>();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Title");
    }

    public void ResetStage()
    {
        SceneManager.LoadScene("FuriousBirds");
    }

    public void SoundVolume()
    {
        isMute = !isMute;
        AudioListener.volume = isMute? 0f : 1f;
    }
}
