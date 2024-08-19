using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("REQUIRED")]

    [SerializeField] private TriggerEvents2D playerTrigger;

    [Space]
    [Header("Enemy Properties")]
    [SerializeField] bool walkOffEdge = true;

    [SerializeField] float damage = 20;
    [SerializeField] float speed = 5;

    [SerializeField] private bool walkingLeft;

    private Rigidbody2D rb;

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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTrigger.OnTriggerEnter += TryDetectPlayer;
    }

    private void TryDetectPlayer(Collider2D collider) 
    {
        
    }


    private void FixedUpdate()
    {
        DetectEdges();

        rb.AddForce(direction * speed * Time.deltaTime);
    }
    
    private void DetectEdges() 
    {
       
    }

    public virtual void OnDetect() 
    {
        
    }
}
