using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomAudioOnAwake : MonoBehaviour
{
    [SerializeField] private AudioClips clips;

    private AudioSource localSrc;
    private void Awake()
    {
        localSrc = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        localSrc.PlayOneShot(clips.GetRandomClip());
    }
}
