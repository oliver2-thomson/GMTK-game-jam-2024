using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBlock : BaseBlock
{
    [Space]
    [Header("Wheel Parameters")]
    [SerializeField] float speedBoost = 30;
    [Space]
    [SerializeField] Animator animator;
    [SerializeField] private float floorAnimationSpeed = 1;
    [SerializeField] private float airAnimationSpeed = 0.1f;
    public bool isGrounded 
    {
        get 
        {
            return colliders.Count > 0;
        }
    }
    HashSet<Collider2D> colliders = new HashSet<Collider2D>();
    PlayerController playerController;

    public override void OnAttachBlock()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        colliders.Add(collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        colliders.Remove(collision);
    }
    public void Update()
    {
        if (AttachedToItem && isGrounded) 
        {
            Vector2 force = new Vector2(playerController.Horizonantal * speedBoost * Time.deltaTime, 0);
            playerController.rb.AddForce(force);
            animator.speed = floorAnimationSpeed;
        }
        else 
        {
            animator.speed = airAnimationSpeed;
        }
    }
}
