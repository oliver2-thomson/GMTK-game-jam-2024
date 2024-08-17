using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    public PlayerAttachment Player;
    public Camera Camera;

    private BaseBlock ObjectGrabbed;

    private bool clickInput;
    private bool scrollUpInput;
    private bool scrollDownInput;

    private void Update()
    {
        clickInput = Input.GetMouseButton(0);
        scrollUpInput = Input.mouseScrollDelta.y > 0 ? true : false;
        scrollDownInput = Input.mouseScrollDelta.y < 0 ? true : false;

        transform.position = Camera.ScreenToWorldPoint(Input.mousePosition);
        Player.grabbedBlock = ObjectGrabbed;

        if (ObjectGrabbed != null)
        {
            if (!clickInput)
            {
                if (ObjectGrabbed.CurrentAttPoint != null)
                {
                    Player.LockInBlockAnimation(ObjectGrabbed.CurrentAttPoint);
                }
                ObjectGrabbed.DragSource = null;
                ObjectGrabbed = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            if (clickInput)
            {
                if (ObjectGrabbed == null)
                {
                    BaseBlock blockComp = collision.gameObject.GetComponent<BaseBlock>();
                    ObjectGrabbed = blockComp;

                    Player.TryDetachBlock(blockComp);
                    blockComp.DragSource = transform;
                    blockComp.TurnOnRigidbody();
                }
            }
        }
    }
}
