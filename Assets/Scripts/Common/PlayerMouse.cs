using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    public float RotationTime;
    [Space(10)]
    public PlayerAttachment Player;
    public Camera Camera;

    private BaseBlock ObjectGrabbed;

    private bool clickInput;
    private bool canRotate = true;

    private void Update()
    {
        clickInput = Input.GetMouseButton(0);

        transform.position = Camera.ScreenToWorldPoint(Input.mousePosition);
        Player.grabbedBlock = ObjectGrabbed;

        if (ObjectGrabbed != null)
        {
            // Rotating the ball
            if (canRotate)
            {
                if (Input.mouseScrollDelta.y != 0)
                {
                    StartCoroutine(RotateObject(Mathf.Sign(Input.mouseScrollDelta.y)));
                }
            }
            
            // Dropping the currently-held ball
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

    private IEnumerator RotateObject(float dir)
    {
        canRotate = false;
        ObjectGrabbed.transform.eulerAngles += new Vector3(0, 0, 90 * dir);

        yield return new WaitForSeconds(RotationTime);
        canRotate = true;
        StopCoroutine(RotateObject(dir));
    }
}
