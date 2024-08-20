using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerBlock : BaseBlock
{
    [Space]
    [Header("Propeller Data")]
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClips blowSFX;
    [Range(0,1)]
    [SerializeField] private float blowSFXVolume = 0.5f;
    [SerializeField] private TriggerEvents2D windTrigger;
    [SerializeField] private float backForce;
    [SerializeField] private float playerForce;
    [SerializeField] private bool toggleTile = false;

    private Animator propelleranimator;
    public override void Awake()
    {
        base.Awake();
        windTrigger.OnTriggerStay += OnWindTunnelEntered;
        propelleranimator = GetComponent<Animator>();
        propelleranimator.speed = toggleTile ? 1 : 0;
        windTrigger.gameObject.SetActive(toggleTile);
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
        if (toggleTile && DragSource == null) 
        {
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (rb != null) 
            {
                if (this.transform.IsChildOf(rb.transform))
                    return;

                if (!audioSrc.isPlaying)
                    audioSrc.PlayOneShot(blowSFX.GetRandomClip(), blowSFXVolume);

                rb.AddForce(backForce * Time.deltaTime * transform.up);
            }
        }
    }
}
