using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : ZoneItem
{
    // Just fades out and loads in a new scene :)
    [Space(10)]
    public int NewSceneIndex;
    public Animator FadeAnimator;
    [SerializeField] private float secondsToTransfer;

    private IEnumerator StartTransition()
    {
        FadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(secondsToTransfer);
        SceneManager.LoadScene(NewSceneIndex);
        StopAllCoroutines();
    }

    public override void OnZoneEnter()
    {
        StartCoroutine(StartTransition());
        Debug.Log("Scene is transitioning");
    }
}
