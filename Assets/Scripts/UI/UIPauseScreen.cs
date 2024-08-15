using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseScreen : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PauseAnim()
    {
        //AudioManager.instance.Play();
        animator.SetTrigger("Pause");
    }

    public void UnpauseAnim()
    {
        //AudioManager.instance.Play();
        animator.SetTrigger("Unpause");
    }
}
