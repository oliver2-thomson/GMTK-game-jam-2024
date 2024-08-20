using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable block = other.GetComponentInParent<Damageable>();
        if (block != null) 
        {
            block.DamageBlock(damage);
        }
    }
}
