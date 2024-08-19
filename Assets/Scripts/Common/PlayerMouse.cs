using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    public float RotationTime;
    [Space(10)]
    public PlayerAttachment Player;
    [SerializeField] private LayerMask attachmentPointLayer;
    [SerializeField] private LineRenderer lineRender;


    private Camera Camera;
    private BaseBlock ObjectGrabbed;
    private bool clickInput;
    private bool canRotate = true;

    private void Start()
    {
        //Stop this from being on the player directly
        //So that Player Center isn't offset weirdly (shouldn't matter but it was annoying me)

        this.transform.parent = null;

        Camera = GameController.instance.Camera;
    }

    private void Update()
    {
        if (GameController.instance.IsEditingBlocks)
        {
            clickInput = Input.GetMouseButton(0);

            transform.position = Camera.ScreenToWorldPoint(Input.mousePosition);

            //Update Line Renderer
            if (lineRender != null)
            {
                lineRender.gameObject.SetActive(ObjectGrabbed != null);
            }


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

                    { //Get Closest Attachment point

                        Collider2D objectCollider = ObjectGrabbed.GetComponent<Collider2D>();

                        RaycastHit2D hitCollider = Physics2D.BoxCast(
                                                objectCollider.bounds.center,
                                                objectCollider.bounds.size,
                                                0,
                                                Vector2.up,
                                                10,
                                                attachmentPointLayer);

                        if (hitCollider.collider != null)
                        {
                            AttachmentPoint attch = hitCollider.collider.GetComponentInParent<AttachmentPoint>();
                            Debug.Log(attch);
                            ObjectGrabbed.CurrentAttPoint = attch;
                        }
                        else
                        {
                            ObjectGrabbed.CurrentAttPoint = null;
                        }
                    }

                    if (ObjectGrabbed.CurrentAttPoint != null)
                    {
                        Player.AttachBlock(ObjectGrabbed.CurrentAttPoint.attachPoint, ObjectGrabbed);
                    }
                    ObjectGrabbed.DragSource = null;
                    ObjectGrabbed = null;
                }
            }
        }
        else 
        {
            lineRender.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameController.instance.IsEditingBlocks)
        {
            if (collision.gameObject.CompareTag("Block"))
            {
                if (clickInput)
                {
                    if (ObjectGrabbed == null)
                    {
                        BaseBlock blockComp = collision.gameObject.GetComponent<BaseBlock>();

                        //DONT TOUCH THE BRAIN!!!
                        if (blockComp.GetType() == typeof(BrainBlock)) 
                        {
                            return;
                        }

                        if (blockComp.AttachedToItem)
                        {
                            if (!Player.TryDetachBlock(blockComp))
                            {
                                return;
                            }
                        }

                        ObjectGrabbed = blockComp;
                        blockComp.DragSource = transform;
                    }
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
