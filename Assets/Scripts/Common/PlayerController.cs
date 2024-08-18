using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Basic platforming physics for the player to use

    // Public variables
    [Header("General Player Settings")]

    [Space(10)]
    [Header("Physics Settings")]
    public float MoveSpeed;
    public float MoveAccel;
    public float MoveMidairAccel;

    public float JumpSpeed;
    public float MaxJumpTime;
    public float Gravity;

    // Private variables
    private bool leftInput;
    private bool rightInput;
    private bool jumpInput;

    private bool isGrounded;
    private float currentJumpTimer;
    private Vector2 currentVelocity;

    // Component references
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private PlayerAttachment playerAttacher;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        playerAttacher = GetComponent<PlayerAttachment>();
    }

    private void Start()
    {
        FindBottomMostPoint();
    }

    private void Update()
    {
        // Processing inputs
        leftInput = Input.GetKey(KeyCode.LeftArrow);
        rightInput = Input.GetKey(KeyCode.RightArrow);
        if (Input.GetKey(KeyCode.Z))
        {
            if (currentJumpTimer == 0)
            {
                jumpInput = true;
            }
            if (!isGrounded)
            {
                currentJumpTimer += Time.deltaTime;
            }
        }
        else
        {
            jumpInput = false;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            FindBottomMostPoint();
        }
    }

    private void FixedUpdate()
    {
        HorizontalMovement();

        GroundCheck();
        CeilingCheck();

        VerticalMovement();

        // Final velocity update
        //TO DO SET VELOCITY TO ADD FORCE
        //COULD DO THIS BY FINDING DIFFERENCE AND CLAMPING IT TO POSITIVE VALUES
        rb.velocity = currentVelocity;
    }

    private void HorizontalMovement()
    {
        if (isGrounded)
        {
            currentVelocity.x = Mathf.MoveTowards(rb.velocity.x, MoveSpeed * (leftInput ? -1 : rightInput ? 1 : 0), MoveAccel);
        }
        else
        {
            currentVelocity.x = Mathf.MoveTowards(rb.velocity.x, MoveSpeed * (leftInput ? -1 : rightInput ? 1 : 0), MoveMidairAccel);
        }
    }

    private void VerticalMovement()
    {
        // Jumping logic
        if (!isGrounded)
        {
            // Keep applying vertical velocity if the jump button gets held down
            if (jumpInput && currentJumpTimer != 0 && currentJumpTimer < MaxJumpTime)
            {
                currentVelocity.y = JumpSpeed;
            }
            else
            {
                // Falling down if there is no jump input
                currentVelocity.y -= Gravity;
            }
        }
        else
        {
            currentVelocity.y = 0;
            if (jumpInput)
            {
                isGrounded = false;
                currentVelocity.y = JumpSpeed;
            }
        }
    }

    private void GroundCheck()
    {
        if (rb.velocity.y <= 0)
        {
            RaycastHit2D[] groundCheck = Physics2D.BoxCastAll(rb.position, col.size, Mathf.Round(transform.eulerAngles.z), Vector2.down);
            foreach (RaycastHit2D hit in groundCheck)
            {
                /*
                if (hit.collider.gameObject.CompareTag("Block"))
                {
                    if (Mathf.Floor(hit.distance * 10) / 10 <= 0 && hit.normal == Vector2.up)
                    {
                        isGrounded = false;
                        break;
                    }
                }
                */
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (hit.normal == Vector2.up)
                    {
                        if (Mathf.Floor(hit.distance * 10) / 10 <= 0)
                        {
                            rb.position = new Vector2(rb.position.x, hit.point.y + (col.size.y / 2));
                            isGrounded = true;
                            currentJumpTimer = 0;
                            break;
                        }
                        else
                        {
                            isGrounded = false;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void CeilingCheck()
    {
        if (rb.velocity.y > 0)
        {
            RaycastHit2D[] ceilingCheck = Physics2D.BoxCastAll(rb.position, col.size, Mathf.Round(transform.eulerAngles.z), Vector2.up);
            foreach (RaycastHit2D hit in ceilingCheck)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (Mathf.Floor(hit.distance * 10) / 10 <= 0 && hit.normal == Vector2.down)
                    {
                        rb.position = new Vector2(rb.position.x, hit.point.y - col.size.y / 2 - 0.1f);
                        currentJumpTimer = MaxJumpTime;
                        currentVelocity.y = -Gravity;
                        break;
                    }
                }
            }
        }
    }

    public void FindBottomMostPoint()
    {
        List<BoxCollider2D> blockChildren = playerAttacher.tileParent.GetComponentsInChildren<BoxCollider2D>().ToList();

        // First, find the blocks which are at the bottom of the "stack", for the new y position
        float lowestY = Mathf.Infinity;
        List<BoxCollider2D> lowestBlocks = new List<BoxCollider2D>();

        foreach (BoxCollider2D block in blockChildren)
        {
            if (block.gameObject.transform.position.y < lowestY)
            {
                lowestY = block.transform.position.y;
            }
        }
        foreach (BoxCollider2D block in blockChildren)
        {
            if (block.gameObject.transform.position.y == lowestY)
            {
                lowestBlocks.Add(block);
            }
        }
        
        // Get rid of blocks that are too far away from each other, so that the legs dont end up dangling in midair
        if (lowestBlocks.Count > 1) 
        {
            for (int i = lowestBlocks.Count - 1; i > 0; i--)
            {
                if ((lowestBlocks[i].transform.position.x - lowestBlocks[i - 1].transform.position.x) > lowestBlocks[i].size.x)
                {
                    lowestBlocks.RemoveAt(i);
                }
            }
        }

        // Deactive the block colliders that are right above the player
        foreach (BoxCollider2D block in blockChildren)
        {
            if (lowestBlocks.Contains(block))
            {
                block.attachedRigidbody.simulated = false;
            }
            else
            {
                block.attachedRigidbody.simulated = true;
            }
        }

        // Then, find the middle point of those for the new x position
        float totalX = 0f;
        foreach (BoxCollider2D block in lowestBlocks)
        {
            totalX += block.transform.position.x;
        }
        float middleX = totalX / lowestBlocks.Count();

        // Move all of the blocks by how much the player would be moving
        
        Vector2 playerNewPos = new Vector2(middleX, lowestY - (lowestBlocks[0].size.y / 2) - (col.size.y / 2));
        playerAttacher.tileParent.localPosition += (Vector3)(rb.position - playerNewPos);
        
        //Todo Stretch lowest block collider to full width to get smooth climbing with bulkier builds
        //col.size = new Vector2(totalX, col.size.y);
    }
}
