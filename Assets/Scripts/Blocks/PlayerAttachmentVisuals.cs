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
        RaycastHit2D hit2D = Physics2D.Raycast(_camera.ScreenPointToRay(Input.mousePosition).origin, _camera.ScreenPointToRay(Input.mousePosition).direction, 100f, MouseInteractions);
        //Stupid Ray cast >:(
        //Debug.DrawRay(_camera.ScreenPointToRay(Input.mousePosition).origin, _camera.ScreenPointToRay(Input.mousePosition).direction, Color.blue, 10);
        if (hit2D.collider != null)
        {
            Debug.Log(hit2D.collider);
            //Check if UI collider or Attachable thing
            AttachmentPoint attacher = hit2D.collider.GetComponentInParent<AttachmentPoint>();
            BaseBlock baseblock = hit2D.collider.GetComponentInParent<BaseBlock>();

            if (grabbedBlock != null && attacher != null)
            {
                LockInBlockAnimation(attacher);
            }
            else if (baseblock != null) 
            {
                if (grabbedBlock) 
                {
                    StopSpinning();
                    DropCurrentObject();
                }


                if (blocksCached.Contains(baseblock) && !baseblock.AttachedToItem) 
                {
                    PickupBlockAnimation(baseblock);
                }        
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
        StartSpinning();
    }

    private void StartSpinning() 
    {
        Beyblade rotate = grabbedBlock.gameObject.AddComponent<Beyblade>();
    }
    private void StopSpinning() 
    {
        Beyblade rotate = grabbedBlock.gameObject.GetComponent<Beyblade>();
        if (rotate) 
        {
            Destroy(rotate);
        }
        else 
        {
            Debug.LogError("Lost spinny thing, not good!");
        }
    }
 
    public void LockInBlockAnimation(AttachmentPoint point) 
    {
        StopSpinning();
        BaseBlock final = grabbedBlock;

        DropCurrentObject();

        AttachBlock(point.attachPoint, final);
    }
}
