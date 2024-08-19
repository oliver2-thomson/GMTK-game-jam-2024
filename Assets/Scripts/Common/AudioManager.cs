using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio manager script for playing different sound effects and music from different objects
    
    public static AudioManager instance;

    public float SoundVolume; // The volume for all of the sound effects
    public float MusicVolume; // The volume for all of the music tracks

    [Space(10)]

    public int MaxSoundLimit;
    List<AudioSource> audioSources = new List<AudioSource>();
    AudioSource musicSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;

        // Create all of the AudioSource components that will be played from the object
        for (int i = 0; i < MaxSoundLimit + 1; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>() as AudioSource;
            audioSources.Add(newSource);
        }

        // The last AudioSource component is reserved for the level's music (ensuring no more than one music track is played at the same time)
        musicSource = audioSources[MaxSoundLimit];
    }

    // Playing sounds/music files

    public IEnumerator PlaySound(string soundName, float volume)
    {
        // Cycling through each AudioSource to see which one is free
        AudioSource mainSource = audioSources[0];
        for (int i = 0; i < MaxSoundLimit - 1; i++)
        {
            if (audioSources[i].clip != null) 
            {
                continue; 
            }
            else
            {
                mainSource = audioSources[i];
                break;
            }
        }
        if (mainSource == null) { yield return null; } // If no AudioSources are found, end the coroutine, no sound plays

        // Get the sound from the 'Resources' folder, play it, and then erase the AudioClip reference
        mainSource.clip = Resources.Load<AudioClip>("Audio/Sounds/" + soundName);

        mainSource.volume = SoundVolume + volume;

        mainSource.Play();
        yield return new WaitForSeconds(mainSource.clip.length);
        mainSource.clip = null;
    }

    public AudioSource GetMusicPlayer()
    {
        AudioSource mainSource = audioSources[MaxSoundLimit];
        return mainSource;
    }


    public void PlayMusic(string musicName, float volume)
    {
        AudioSource mainSource = audioSources[MaxSoundLimit];

        // Get the music from the 'Resources' folder, play it, and then erase the AudioClip reference
        mainSource.clip = Resources.Load<AudioClip>("Audio/Music/" + musicName);

        mainSource.volume = MusicVolume + volume;
        mainSource.loop = true;

        mainSource.Play();
    }

    // Stopping all instances of sound/music

    public void StopAllSounds()
    {
        foreach (AudioSource sound in audioSources)
        {
            sound.clip = null;
        }
    }

    public void StopMusic()
    {
        musicSource.clip = null;
    }
}
