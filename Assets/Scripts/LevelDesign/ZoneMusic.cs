using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMusic : ZoneAmb
{
    private void Awake()
    {

    }
    public override void OnZoneEnter()
    {
        AudioSource src = AudioManager.instance.GetMusicPlayer();
        src.loop = isLooping;

        if (zoneAudio == null)
        {
            StartCoroutine(StartFade(src, fadeOutTime, 0));
        }
        else
        {
            if (src.clip == zoneAudio && !src.isPlaying) 
            {
                return;
            }

            src.clip = zoneAudio;
            src.Play();
            StartCoroutine(StartFade(src, fadeInTime, volume));
        }
    }
    public override void OnZoneExit()
    {
       
    }
}
