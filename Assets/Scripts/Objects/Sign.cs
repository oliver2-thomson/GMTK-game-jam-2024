using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public float SignFadeSpeed;

    private Color startColor;
    private bool fadeIn;
    private bool fadeOut;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startColor = sr.color;
        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeIn = true;
            fadeOut = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeOut = true;
            fadeIn = false;
        }
    }

    private void FixedUpdate()
    {
        if (fadeIn)
        {
            if (sr.color.a < 1)
            {
                sr.color += new Color(startColor.r, startColor.g, startColor.b, SignFadeSpeed);
            }
            else
            {
                fadeIn = false;
            }
        }
        else if (fadeOut)
        {
            if (sr.color.a > 0)
            {
                sr.color -= new Color(startColor.r, startColor.g, startColor.b, SignFadeSpeed);
            }
            else
            {
                fadeOut = false;
            }
        }
    }
}
