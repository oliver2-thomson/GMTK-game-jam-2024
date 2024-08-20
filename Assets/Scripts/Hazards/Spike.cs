using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private float damage = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseBlock block = other.GetComponent<BaseBlock>();
        if (block != null) 
        {
            block.DamageBlock(damage);
        }
    }
}
