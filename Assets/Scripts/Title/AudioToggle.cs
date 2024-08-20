using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    // Display for toggling music/sound volume
    public bool IsMusicToggle;
    public bool IsSoundToggle;

    private TMP_Text textComp;

    private void Awake()
    {
        textComp = GetComponent<TMP_Text>();
    }

    public void ToggleVolume(int dir)
    {
        if (IsMusicToggle)
        {
            AudioManager.instance.MusicVolume += dir * 0.1f;
            if (AudioManager.instance.MusicVolume > 1)
            {
                AudioManager.instance.MusicVolume = 0;
            }
            textComp.text = (Mathf.Round(AudioManager.instance.MusicVolume * 100)).ToString() + "%";
        }
        if (IsSoundToggle)
        {
            AudioManager.instance.SoundVolume += dir * 0.1f;
            if (AudioManager.instance.SoundVolume > 1)
            {
                AudioManager.instance.SoundVolume = 0;
            }
            textComp.text = (Mathf.Round(AudioManager.instance.SoundVolume * 100)).ToString() + "%";
        }
    }
}
