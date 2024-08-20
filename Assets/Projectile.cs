using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    private float damage = 0;
    private Transform owner;
    public void GimmeDamage(float PutDamageHere, Transform ownerOrigin) 
    {
        damage = PutDamageHere;
        owner = ownerOrigin;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner == null)
        {
            Debug.LogError("GIMME DAMAGE HASNT BEEN CALLED ");
        }
        if (collision.isTrigger || collision.transform.IsChildOf(owner)) 
        {
            return;
        }

        Damageable damagable = collision.GetComponentInParent<Damageable>();
        if (damagable != null) 
        {
            damagable.DamageAtPoint(collision.ClosestPoint(transform.position), damage);
        }

        GameObject.Instantiate(explosion.gameObject, transform.position, transform.rotation, null);
        Destroy(this.gameObject);
    }
}
