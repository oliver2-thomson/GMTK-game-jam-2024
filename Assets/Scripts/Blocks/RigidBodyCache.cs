

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RigidBodyCache : MonoBehaviour
{
    [Tooltip("Recalculate Cache of rigidbody each time it's destroyed?")]
    [SerializeField] private bool ResetCacheOnRespawn = true;

    [HideInInspector] public Rigidbody2D rb;

    bool CachedRigidbodyData = false;
    //Rigidbody Data:
    RigidbodyType2D bodyType;
    PhysicsMaterial2D sharedMaterial;
    bool useAutoMass;

    float mass;
    float drag;
    float angularDrag;
    float gravityScale;

    CollisionDetectionMode2D collisionDetectionMode;
    RigidbodyInterpolation2D interpolation;
    RigidbodyConstraints2D constraints2D;
    bool freezeRotation;

    LayerMask includeLayers, excludeLayers;


    //Do I need to cache this??
    Vector2 centerOfMass;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        CacheRigidbodyData();
    }

    private void CacheRigidbodyData() 
    {
        bodyType = rb.bodyType;
        sharedMaterial = rb.sharedMaterial;
        useAutoMass = rb.useAutoMass;

        mass = rb.mass;
        drag = rb.drag;
        angularDrag = rb.angularDrag;
        gravityScale = rb.gravityScale;

        
        collisionDetectionMode = rb.collisionDetectionMode;
        interpolation = rb.interpolation;

        //Constraints
        constraints2D = rb.constraints;
        freezeRotation = rb.freezeRotation;

        //Layer Overrides
        includeLayers = rb.includeLayers;
        excludeLayers = rb.excludeLayers;

        //DO I NEED THIS?!?!?
        centerOfMass = rb.centerOfMass;

        CachedRigidbodyData = true;
    }


    private void PasteRigidbody() 
    {
        // Reassigning the cached variables back to rb
        rb.bodyType = bodyType;
        rb.sharedMaterial = sharedMaterial;
        rb.useAutoMass = useAutoMass;
        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
        rb.gravityScale = gravityScale;
        rb.collisionDetectionMode = collisionDetectionMode;
        rb.interpolation = interpolation;

        // Constraints
        rb.constraints = constraints2D;
        rb.freezeRotation = freezeRotation;

        // Layer Overrides
        rb.includeLayers = includeLayers;
        rb.excludeLayers = excludeLayers;

        // Center of Mass
        rb.centerOfMass = centerOfMass;

    }

    [ContextMenu("Delete Rigidbody")]
    public void DeleteOldRigidbody() 
    {
        if (!CachedRigidbodyData) 
        {
            CacheRigidbodyData();
        }
        Destroy(rb);
    }

    [ContextMenu("Recreate Rigidbody")]
    public void RecreateRigidbody() 
    {
        if (!CachedRigidbodyData)
        {
            Debug.LogError($"Cannot recreate rigidbody, cache doesn't exist! - {gameObject.name}");
            return;
        }

        if (ResetCacheOnRespawn) 
        {
            CachedRigidbodyData = false;
        }

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        PasteRigidbody();
    }
}
