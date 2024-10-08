using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents2D : MonoBehaviour
{
    public System.Action<Collider2D> OnTriggerEnter;
    public System.Action<Collider2D> OnTriggerStay;
    public System.Action<Collider2D> OnTriggerExit;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay?.Invoke(collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(collision);
    }
}
