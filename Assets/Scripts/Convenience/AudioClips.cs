using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiSounds", menuName = "Audio/MultiClips")]
public class AudioClips : ScriptableObject
{
    [SerializeField] public AudioClip[] audioClips;

    public AudioClip GetRandomClip() 
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }
}
