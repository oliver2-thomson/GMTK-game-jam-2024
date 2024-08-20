using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents2DToggle : TriggerEvents2D
{
    public bool Toggled 
    {
        get 
        {
            return colliders.Count > 0;
        }
    }

    private HashSet<Collider2D> colliders = new HashSet<Collider2D>();

    public new void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
        OnTriggerEnter?.Invoke(collision);
    }
    public new void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
        OnTriggerExit?.Invoke(collision);
    }
}
