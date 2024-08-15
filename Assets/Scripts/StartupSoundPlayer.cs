using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "FunnySound", menuName = "Labworks/FunnyDevPranks")]
public class StartupSoundPlayer : ScriptableObject
{

    public List<AudioClip> funnySounds = new List<AudioClip>();


    [InitializeOnLoadMethod]
    static void PlayStartupSound()
    {
        string soundPath = "Assets/HeHeHeHa.asset"; // Change this to your sound file path
        StartupSoundPlayer clips = AssetDatabase.LoadAssetAtPath<StartupSoundPlayer>(soundPath);


        
        if (clips != null && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            GameObject game = new GameObject();
            game.hideFlags = HideFlags.HideAndDontSave;

            AudioSource bootSource = game.AddComponent<AudioSource>();
            bootSource.PlayOneShot(clips.funnySounds[Random.Range(0, clips.funnySounds.Count - 1)]);

            //DestroyImmediate(game); s
        }
        else
        {
            Debug.LogError("Sound file not found at path: " + soundPath);
        }


    }
}

#endif