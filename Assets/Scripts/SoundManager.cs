using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Audio Clips
    public AudioClip gameBGM;
    public AudioClip audioCustomSat;
    public AudioClip audioCustomWrong;
    public AudioClip audioCustomLate;
    public AudioClip audioTouchTray;
    public AudioClip audioFryingFan;
    public AudioClip audioGetStar;

    public AudioClip audioButtons;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string action)
    {
        switch (action)
        {
            case "CUSTOM_SAT":
                audioSource.clip = audioCustomSat;
                break;
            case "CUSTOM_LATE":
                audioSource.clip = audioCustomLate;
                break;
            case "CUSTOM_WRONG":
                audioSource.clip = audioCustomWrong;
                break;
            case "TOUCHTRAY":
                audioSource.clip = audioTouchTray;
                break;
            case "GETSTAR":
                audioSource.clip = audioGetStar;
                break;

        }
        audioSource.Play();
    }

    public void ButtonSound()
    {
        audioSource.clip = audioButtons;
        audioSource.Play();
    }

    public void NormalOrderClear()
    {
        audioSource.clip = audioCustomSat;
        audioSource.Play();
    }

    public void LateOrder()
    {
        audioSource.clip = audioCustomLate;
        audioSource.Play();
    }

    public void WrongOrder()
    {
        audioSource.clip = audioCustomWrong;
        audioSource.Play();
    }

    public void SpecialOrderClear()
    {
        audioSource.clip = audioGetStar;
        audioSource.Play();
    }
}
