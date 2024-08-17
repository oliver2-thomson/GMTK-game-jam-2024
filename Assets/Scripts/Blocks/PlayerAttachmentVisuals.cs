using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttachment : MonoBehaviour
{
    [Space]
    [Header("Pickup Attributes")]
    [SerializeField] TriggerEvents2D trigger;
    [SerializeField] private Camera _camera;
    [SerializeField] LayerMask MouseInteractions;

    [SerializeField] Transform pickupCube;
    [SerializeField] float pickupRotateSpeed;

    private HashSet<BaseBlock> blocksCached = new HashSet<BaseBlock>();

    public BaseBlock grabbedBlock;
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

    public void DropCurrentObject() 
    {
        HideUI();
        grabbedBlock.transform.parent = null;
        grabbedBlock.transform.rotation = Quaternion.identity;
        grabbedBlock = null;
    }

    public void PickupBlockAnimation(BaseBlock block) 
    {
        ShowAttachmentUI();
        block.transform.parent = pickupCube;
        block.transform.localPosition = Vector3.zero;
        grabbedBlock = block;
    }
 
    public void LockInBlockAnimation(AttachmentPoint point) 
    {
        BaseBlock final = grabbedBlock;

        DropCurrentObject();

        AttachBlock(point.attachPoint, final);
    }
}
