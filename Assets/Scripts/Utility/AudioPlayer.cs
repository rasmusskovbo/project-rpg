using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip shootingSFX;
    [SerializeField] [Range(0f, 1f)] private float shootingFXVolume = 0.25f;
    
    private bool isAudioEnabled = true;
    
    // Static instancess version of Singleton pattern
    private static AudioPlayer instance;
    
    private void Awake()
    {
        if (isAudioEnabled) GetComponent<AudioSource>().Play();
        ManageSingleton();
    }

    void ManageSingleton()
    {
        /* Private Singleton pattern
         * int instanceCount = FindObjectsOfType(GetType()).Length;
         * if (instanceCount > 1)
         */

        if (instance != null && isAudioEnabled)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetAudio()
    {
        isAudioEnabled = !isAudioEnabled;
        AudioSource theme = GetComponent<AudioSource>();
        if (theme.isPlaying)
        {
            theme.Stop();
        }
        else
        {
            theme.Play();
        }
    }

    /* Used for Static version of Singleton pattern
    public AudioPlayer GetInstance()
    {
        return instance;
    }
    */

    /*
    public void PlayShootingFX()
    {
        if (shootingSFX != null && isAudioEnabled && isLaserSoundsEnabled)
        {
            AudioSource.PlayClipAtPoint(
                shootingSFX, 
                Camera.main.transform.position, 
                shootingFXVolume
            );
        }
    }
    */
    
    
}
