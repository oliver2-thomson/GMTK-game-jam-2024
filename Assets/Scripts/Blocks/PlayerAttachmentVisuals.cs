using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttachment : MonoBehaviour
{
    [Space]
    [Header("Pickup Attributes")]
    [SerializeField] TriggerEvents2D trigger;
    [SerializeField] private Camera _camera;

    private HashSet<BaseBlock> blocksCached = new HashSet<BaseBlock>();
    private void StartVisuals() 
    {
        trigger.OnTriggerEnter += OnPlatformTriggerEnter;
        trigger.OnTriggerExit += OnPlatformTriggerExit;
    }

    private void OnPlatformTriggerEnter(Collider2D collider) 
    {
        BaseBlock block = collider.GetComponentInParent<BaseBlock>();
        if (block) 
        {
            if (!blocksCached.Contains(block)) 
            {
                blocksCached.Add(block);
            }
        }
    }
    private void OnPlatformTriggerExit(Collider2D collider)
    {
        BaseBlock block = collider.GetComponentInParent<BaseBlock>();
        if (block)
        {
            if (!blocksCached.Contains(block))
            {
                blocksCached.Remove(block);
            }
        }
    }

    public bool IsBlockWithinDistance(BaseBlock block) 
    {
        return blocksCached.Contains(block);
    }
}
