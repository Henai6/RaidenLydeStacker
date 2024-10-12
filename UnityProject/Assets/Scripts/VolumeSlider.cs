using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeSlider : MonoBehaviour
{

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVolume(float volume)
    {
        SoundMaster.globalVolume = volume;
        audioSource.Play();
    }
}
