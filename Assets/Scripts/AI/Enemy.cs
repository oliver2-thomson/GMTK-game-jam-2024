using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{

    [Header("REQUIRED")]

    [SerializeField] private AudioClips DeathSFX;
    [SerializeField] private TriggerEvents2D playerTrigger;
    [SerializeField] private Vector2 colliderCheckSize;
    [SerializeField] public LayerMask wallChecks;

    [Space]
    [Header("Enemy Properties")]
    [SerializeField] private GameObject[] BlocksDropped;
    [SerializeField] private float blockExplosionValue = 10;
    [SerializeField] bool walkOffEdge = true;
    [SerializeField] bool flipTheFlip = false;
    [SerializeField] public float speed = 5;
    [SerializeField] public bool walkingLeft;

    [Space]
    [SerializeField] private bool VelocityBased = true;
    [SerializeField] private float velocityScale = 0.1f;
    [SerializeField] float damage = 20;
    [SerializeField] private float knockback = 2f;

    [SerializeField] private Transform debugBox;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D localCollider;
    private SpriteRenderer sprite;

    [HideInInspector] public System.Action _OnDeath;
    [HideInInspector] public bool IgnoreRationality = false;

    [HideInInspector] public PlayerAttachment _player;

    public Vector2 direction 
    {
        get 
        {
            if (walkingLeft) 
            {
                return Vector2.left;
            }
            else 
            {
                return Vector2.right;
            }
        }
    }


    public override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        playerTrigger.OnTriggerEnter += TryDetectPlayer;
        playerTrigger.OnTriggerExit += TryLosePlayer;
        localCollider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        Physics2D.queriesHitTriggers = false;
    }

    private void TryDetectPlayer(Collider2D collider) 
    {
        PlayerAttachment player = collider.GetComponentInParent<PlayerAttachment>();

        if (player != null) 
        {
            OnDetect(player);
        }
    }

    private void TryLosePlayer(Collider2D collider)
    {
        PlayerAttachment player = collider.GetComponentInParent<PlayerAttachment>();

        if (player != null)
        {
            OnLosePlayer(player);
        }
    }


    private void FixedUpdate()
    {
        if (flipTheFlip)
        {
            sprite.flipX = !walkingLeft;
        }
        else
        {
            sprite.flipX = walkingLeft;
        }

        if (IgnoreRationality)
        {
            UpdateDetect();
            return;
        }

        DetectEdges();

        rb.AddForce(direction * speed * Time.deltaTime);
    }
    
    private void DetectEdges() 
    {
        Vector3 positionOffset = localCollider.bounds.center;
        positionOffset.x += (localCollider.bounds.size.x * direction.x);



        RaycastHit2D rayHit = Physics2D.BoxCast(new Vector2(positionOffset.x, positionOffset.y), 
            colliderCheckSize, 0, direction, 0, wallChecks);

        if (debugBox != null)
            debugBox.position = positionOffset;

        if (rayHit.collider != null)
        {
            //Connected Block to player edge case
            BaseBlock block = rayHit.collider.GetComponent<BaseBlock>();
            if (block != null && block.AttachedToItem) 
            {
                return;
            }

            walkingLeft = !walkingLeft;
        }
    }

    public virtual void UpdateDetect() 
    {

    }

    public virtual void OnDetect(PlayerAttachment player) 
    {
        
    }

    public virtual void OnLosePlayer(PlayerAttachment player)
    {
        
    }

    public override void OnDeath()
    {
        _OnDeath?.Invoke();

        AudioSource.PlayClipAtPoint(DeathSFX.GetRandomClip(), transform.position);

       foreach (GameObject block in BlocksDropped) 
        {
            GameObject clone = GameObject.Instantiate(block, transform.position, transform.rotation, null);
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            
            Vector2 RandomDirection = new Vector2(Random.Range(0, 1), Random.Range(0, 1)).normalized;

            rb.AddForce(RandomDirection * blockExplosionValue);
        }

        GameObject.Destroy(this.gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        BaseBlock block = collision.collider.GetComponent<BaseBlock>();
        if (block != null)
        {
            float velocityDamage;

            //Calculate Damage
            if (VelocityBased)
            {
                velocityDamage = damage * velocityScale * GetComponentInParent<Rigidbody2D>().velocity.magnitude;
            }
            else
                velocityDamage = damage;

            //Knock back THIS IS JANK PLZ FIX
            Rigidbody2D blockRb = block.GetComponentInParent<Rigidbody2D>();
            blockRb.velocity += (rb.velocity.normalized * knockback);

            block.DamageAtPoint(collision.GetContact(0).point, velocityDamage);
        }
    }
}
