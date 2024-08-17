using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAmb : ZoneItem
{
    [Header("Zone Ambience Use this for Music/3D Sound")]
    [SerializeField] AudioClip zoneAudio;
    
    [Space]
    [SerializeField] float fadeInTime = 0;
    [SerializeField] float fadeOutTime = 0;
    
    [Space]
    
    [SerializeField] bool isLooping = false;

    [Range(0,1)]
    [SerializeField] float volume = 0.5f;

    [Range(0,1)]
    [SerializeField] float spatialBlend = 0;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.clip = zoneAudio;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend;
        audioSource.loop = isLooping;
    }

    public void SetAudioVolume(float volume) 
    {
        audioSource.volume = volume;
    }

    public override void OnZoneEnter()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            if (fadeInTime > 0)
            {
                audioSource.volume = 0;
                StartCoroutine(StartFade(audioSource, fadeInTime, volume));
            }
        }
    }
    public override void OnZoneExit()
    {
        StartCoroutine(StartFade(audioSource, fadeOutTime, 0));
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        if (targetVolume == 0) 
        {
            audioSource.Stop();
        }

        yield break;
    }
}
