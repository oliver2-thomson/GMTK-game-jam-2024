using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pogo : MonoBehaviour
{
    bool hitSuccessful = false;
    bool onCoolDown = false;
    [SerializeField] float upwardForce = 300;
    [SerializeField] float hitForce = 500;
    [SerializeField] float resetTimer = 3;
    [SerializeField] private BoxCollider2D boxTrigger;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClips SFX;

    private void Start()
    {
        boxTrigger.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) 
        {
            //WHY DO I NEED TO DO THIS?!?!
            return;
        }

        Rigidbody2D rigidParent = collision.transform.GetComponentInParent<Rigidbody2D>();
        if (rigidParent == null ||
            CheckIsNotSelf(rigidParent.transform))
        {
            return;
        }

        hitSuccessful = true;
        //Apply force to hit object
        rigidParent.AddForce(hitForce * transform.up);
    }

    public void UsePogo() 
    {
        if (onCoolDown) 
        {
            return;
        }

        StartCoroutine(PogoTimer());

        

    }

    private IEnumerator PogoCooldown() 
    {
        float timer = 0;

        while(timer < resetTimer) 
        {
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        onCoolDown = false;
    }


    private IEnumerator PogoTimer() 
    {
        hitSuccessful = false;
        boxTrigger.enabled = true;
        yield return new WaitForFixedUpdate();
        boxTrigger.enabled = false;

        if (hitSuccessful) 
        {
            GetComponentInParent<Rigidbody2D>().velocity += (Vector2)(upwardForce * -transform.up);

            animator.Play("Pogo");
            AudioSource.PlayClipAtPoint(SFX.GetRandomClip(), transform.position);

            onCoolDown = true;
            StartCoroutine(PogoCooldown());
        }
    }

    private bool CheckIsNotSelf(Transform obj) 
    {
        return this.transform.IsChildOf(obj) || obj.transform.IsChildOf(transform);
    }
}
