using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile proj = collision.GetComponent<Projectile>();
        if (proj != null) 
        {
            proj.owner = this.GetComponentInParent<Rigidbody2D>().transform;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.velocity *= -1;
        }
    }
}
