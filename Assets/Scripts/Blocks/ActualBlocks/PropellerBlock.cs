using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerBlock : BaseBlock
{
    [Space]
    [Header("Propeller Data")]
    [SerializeField] private TriggerEvents2D windTrigger;
    [SerializeField] private float backForce;
    [SerializeField] private float playerForce;
    [SerializeField] private bool toggleTile = false;

    private Animator propelleranimator;
    private void Awake()
    {
        windTrigger.OnTriggerStay += OnWindTunnelEntered;
        propelleranimator = GetComponent<Animator>();
        propelleranimator.speed = toggleTile ? 1 : 0;
    }

    public override void OnToggleTile()
    {
        toggleTile = !toggleTile;
        windTrigger.gameObject.SetActive(toggleTile);
        propelleranimator.speed = toggleTile? 1 : 0;
    }

    private void Update()
    {
        if (toggleTile) 
        {
            BlowPlayer();    
        }
    }

    private void BlowPlayer() 
    {
        Rigidbody2D rb = gameObject.GetComponentInParent<Rigidbody2D>();
        rb.AddForce(playerForce * Time.deltaTime * -transform.up);
    }

    private void OnWindTunnelEntered(Collider2D collision) 
    {
        if (toggleTile) 
        {
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (rb != null) 
            {
                if (this.transform.IsChildOf(rb.transform))
                    return;

                rb.AddForce(backForce * Time.deltaTime * transform.up);
            }
        }
    }
}
