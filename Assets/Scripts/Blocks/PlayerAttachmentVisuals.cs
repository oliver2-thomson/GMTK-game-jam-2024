using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class PlayerAttachment : MonoBehaviour
{
    [Space]
    [Header("Pickup Attributes")]
    [SerializeField] TriggerEvents2D trigger;
    [SerializeField] LayerMask MouseInteractions;

    [SerializeField] Transform pickupCube;
    [SerializeField] float pickupRotateSpeed;

    private HashSet<BaseBlock> blocksCached = new HashSet<BaseBlock>();

    private BaseBlock grabbedBlock;
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

    private void Update()
    {
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        
        if (mouseButtonDown) 
        {
            RaycastToFindShit();
        }
    }

    private void RaycastToFindShit()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f, MouseInteractions);

        if (hit2D)
        {
            //Check if UI collider or Attachable thing
            AttachmentPoint attacher = hit2D.collider.GetComponentInParent<AttachmentPoint>();
            BaseBlock baseblock = hit2D.collider.GetComponentInParent<BaseBlock>();

            if (attacher != null)
            {
                LockInBlockAnimation(attacher);
            }
            else if (baseblock != null) 
            {
                if (grabbedBlock) 
                {
                    //StopCoroutine(SpinPickedObject());
                    DropCurrentObject();
                }
                Debug.Log("hit object");

                if (blocksCached.Contains(baseblock)) 
                {
                    PickupBlockAnimation(baseblock);
                }        
            }
        }
    }


    public void DropCurrentObject() 
    {
        grabbedBlock.transform.parent = null;
        grabbedBlock.transform.rotation = Quaternion.identity;
        grabbedBlock = null;
    }

    public void PickupBlockAnimation(BaseBlock block) 
    {
        ShowAttachmentUI();
        block.transform.parent = pickupCube.parent;
        block.transform.localPosition = Vector3.zero;
        //StartCoroutine(SpinPickedObject());
    }

 
    public void LockInBlockAnimation(AttachmentPoint point) 
    {
        //StopCoroutine(SpinPickedObject());
        AttachBlock(point.attachPoint, grabbedBlock);

    }
}
