using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : BaseBlock
{
    [Space]
    [Header("Spike Data")]
    [SerializeField] private Vector2 offsetTowardsCenter;
    [SerializeField] private float damage = 1.5f;
    [SerializeField] private AudioClips DamageSFX;

    [Space]
    [SerializeField] private bool VelocityBased = true;
    [SerializeField] private float velocityScale = 0.1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable block = collision.collider.GetComponentInParent<Damageable>();
        if (block != null && !CheckObjectIsntAttached(collision.transform))
        {
            //Calculate damage position
            Vector2 pos = collision.GetContact(0).point;
            pos -= offsetTowardsCenter;


            float velocityDamage;

            //Calculate Damage
            if (VelocityBased)
            {
                velocityDamage = damage * velocityScale * GetComponentInParent<Rigidbody2D>().velocity.magnitude;
            }
            else
                velocityDamage = damage;

            AudioSource.PlayClipAtPoint(DamageSFX.GetRandomClip(), transform.position);

            block.DamageAtPoint(pos, velocityDamage);
        }
    }


    // UNUSED!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable block = collision.GetComponentInParent<Damageable>();
        if (block != null && CheckObjectIsntAttached(collision.transform))
        {
            //Calculate damage position
            Vector2 pos = collision.ClosestPoint(block.transform.position);
            pos -= offsetTowardsCenter;


            float velocityDamage;

            //Calculate Damage
            if (VelocityBased)
            {
                velocityDamage = damage * velocityScale * GetComponentInParent<Rigidbody2D>().velocity.magnitude;
            }
            else
                velocityDamage = damage;

            block.DamageAtPoint(pos, velocityDamage);
        }
    }
}
