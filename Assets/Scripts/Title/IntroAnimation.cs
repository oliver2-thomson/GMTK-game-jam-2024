using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroAnimation : MonoBehaviour
{
    // Intro sequence script, for getting the player's input focus, and to play a titlecard animation before going into the title-screen

    public Animation TitleAnimation;
    private Button startButton;

    private void Awake()
    {
        startButton = FindObjectOfType<Button>();
    }

    public void IntroStart()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        if (TitleAnimation != null)
        {
            TitleAnimation.Play();
            yield return new WaitForSeconds(TitleAnimation.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene(1);
    }
}
