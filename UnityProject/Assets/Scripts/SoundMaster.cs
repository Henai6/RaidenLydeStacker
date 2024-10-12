using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundMaster : MonoBehaviour
{
    public static float globalVolume = 1f;

    [System.Serializable]
    public class AudioClipData
    {
        public AudioClip clip;
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
    }

    // List of AudioClipData that stores clips and their volumes
    public List<AudioClipData> audioClips;

    // Cached reference to a single AudioSource for playing sounds
    private AudioSource audioSource;

    private void Awake()
    {
        // Create and configure the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Play the audio clip by index
    public void Play(int index)
    {
        if (index < 0 || index >= audioClips.Count)
        {
            Debug.LogWarning("AudioManager: Invalid clip index.");
            return;
        }

        AudioClipData clipData = audioClips[index];
        audioSource.PlayOneShot(clipData.clip, clipData.volume * globalVolume);
    }
}
