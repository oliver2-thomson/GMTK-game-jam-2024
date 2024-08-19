using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : Enemy
{
    [SerializeField] private float bounceDistance = 0.03f;
    [SerializeField] private float bounceAmount = 40;
    [SerializeField] private float bounceMultipler = 1.2f;

    private float currentBounce;

    private void Start()
    {
        currentBounce = bounceAmount;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger || collision.transform.IsChildOf(transform)) 
        {
            return;
        }

        currentBounce = bounceAmount;

    }
    private void Update()
    {
        if (rb.velocity.y < 0) 
        {
            if (CheckGrounded()) 
            {
                currentBounce *= bounceMultipler;
                rb.AddForce(new Vector2(0, currentBounce));                
            }
        }
    }

    private bool CheckGrounded() 
    {
        Vector3 positionOffset = localCollider.bounds.center;
        positionOffset.y -= ((localCollider.bounds.size.y / 2) + bounceDistance);
        return Physics2D.Raycast(positionOffset, Vector2.down, bounceDistance, wallChecks).collider != null;
    }
}
