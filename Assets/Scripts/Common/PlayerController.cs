using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Basic platforming physics for the player to use

    // Public variables
    [Header("General Player Settings")]
    public bool InputEnabled;

    [Space(10)]
    [Header("Physics Settings")]
    public float MoveSpeed;
    public float MoveAccel;
    public float MoveMidairAccel;

    public float JumpSpeed;
    public float MaxJumpTime;
    public float StackSpeedBoost;

    public float Horizonantal 
    {
        get 
        {
            return (leftInput ? -1 : rightInput ? 1 : 0);
        }
    }

    // Private variables
    private bool leftInput;
    private bool rightInput;
    private bool jumpInput;

    private bool isGrounded;
    private float currentJumpTimer;
    private float blockNum;
    private float speedBoostForce;

    // Component references
    [HideInInspector] public Rigidbody2D rb;
    private BoxCollider2D col;
    private CompositeCollider2D compCol;
    private PlayerAttachment playerAttacher;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        compCol = GetComponent<CompositeCollider2D>();
        playerAttacher = GetComponent<PlayerAttachment>();
    }

    private void Start()
    {
        FindBottomMostPoint();
    }

    private void Update()
    {
        // Processing inputs
        if (InputEnabled)
        {
            leftInput = Input.GetKey(KeyCode.A);
            rightInput = Input.GetKey(KeyCode.D);
            if (Input.GetKey(KeyCode.W))
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
        }

        blockNum = 0;
        foreach (RigidBodyCache block in playerAttacher.tileParent.GetComponentsInChildren<RigidBodyCache>())
        {
            blockNum += block.mass;
        }
        speedBoostForce = (blockNum * StackSpeedBoost);
    }

    private void FixedUpdate()
    {
        HorizontalMovement();

        GroundCheck();
        CeilingCheck();

        VerticalMovement();
    }

    private void HorizontalMovement()
    {
        float targetSpeed = MoveSpeed * Horizonantal;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelRate = isGrounded ? MoveAccel : MoveMidairAccel;
        float finalForce = speedDifference * accelRate;

        // Apply the horizontal force
        rb.velocity += new Vector2(finalForce * Time.deltaTime, 0);
        //rb.AddForce(new Vector2(finalForce * (blockNum > 1 ? speedBoostForce * 2: 1), 0), ForceMode2D.Force);
    }

    private void VerticalMovement()
    {
        // Jumping logic
        if (!isGrounded)
        {
            if (jumpInput && currentJumpTimer != 0 && currentJumpTimer < MaxJumpTime)
            {
                rb.AddForce(new Vector2(0, (JumpSpeed + (blockNum > 1 ? blockNum : 1)) - rb.velocity.y), ForceMode2D.Impulse);
            }
            else
            {
                //rb.AddForce(new Vector2(0, -rb.gravityScale), ForceMode2D.Force);
            }
        }
        else
        {
            if (jumpInput)
            {
                isGrounded = false;
                rb.AddForce(new Vector2(0, JumpSpeed + (blockNum > 1 ? blockNum : 1)), ForceMode2D.Impulse);
            }
        }
    }

    private List<BoxCollider2D> GetRelevantColliderObjects()
    {
        List<BoxCollider2D> colCheckObjects = playerAttacher.tileParent.GetComponentsInChildren<BoxCollider2D>().ToList();

        // Only check colliders which are for the blocks themselves
        for (int i = colCheckObjects.Count - 1; i >= 0; i--)
        {
            if (colCheckObjects[i].GetComponent<BaseBlock>() == null)
            {
                colCheckObjects.Remove(colCheckObjects[i]);
            }
        }

        colCheckObjects.Add(col);
        return colCheckObjects;
    }

    private void GroundCheck()
    {
        if (Mathf.Floor(rb.velocity.y * 10) / 10 <= 0)
        {
            List<BoxCollider2D> colCheckObjects = GetRelevantColliderObjects();

            foreach (BoxCollider2D block in colCheckObjects)
            {
                bool colCheckFinished = false;

                RaycastHit2D[] groundCheck = Physics2D.BoxCastAll(block.transform.position, block.size, Mathf.Round(transform.eulerAngles.z), Vector2.down);

                foreach (RaycastHit2D hit in groundCheck)
                {
                    if (hit.collider.gameObject != block.gameObject)
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                        {
                            if (hit.normal == Vector2.up)
                            {
                                if (Mathf.Floor(hit.distance * 10) / 10 <= 0)
                                {
                                    isGrounded = true;
                                    currentJumpTimer = 0;
                                    colCheckFinished = true;
                                    break;
                                }
                                else
                                {
                                    isGrounded = false;
                                    break;
                                }
                            }
                        }
                        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
                        {
                            BaseBlock testBlock = hit.collider.gameObject.GetComponent<BaseBlock>();
                            if (testBlock != null)
                            {
                                if (!testBlock.AttachedToItem)
                                {
                                    if (hit.normal == Vector2.up)
                                    {
                                        if (Mathf.Floor(hit.distance * 10) / 10 <= 0)
                                        {
                                            isGrounded = true;
                                            currentJumpTimer = 0;
                                            colCheckFinished = true;
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
                }

                if (colCheckFinished)
                {
                    break;
                }
            }
        }
    }

    private void CeilingCheck()
    {
        if (rb.velocity.y > 0)
        {
            List<BoxCollider2D> colCheckObjects = GetRelevantColliderObjects();

            foreach (BoxCollider2D block in colCheckObjects)
            {
                bool colCheckFinished = false;

                RaycastHit2D[] ceilingCheck = Physics2D.BoxCastAll(block.transform.position, block.size, Mathf.Round(transform.eulerAngles.z), Vector2.up);
                foreach (RaycastHit2D hit in ceilingCheck)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        if (Mathf.Floor(hit.distance * 10) / 10 <= 0 && hit.normal == Vector2.down)
                        {
                            currentJumpTimer = MaxJumpTime;
                            rb.AddForce(new Vector2(rb.velocity.x, -rb.gravityScale), ForceMode2D.Force);
                            colCheckFinished = true;
                            break;
                        }
                    }
                }

                if (colCheckFinished)
                {
                    break;
                }
            }
        }
    }

    public void FindBottomMostPoint()
    {
        List<BoxCollider2D> blockChildren = playerAttacher.tileParent.GetComponentsInChildren<BoxCollider2D>().ToList();
        // Only check colliders which are for the blocks themselves
        for (int i = blockChildren.Count - 1; i >= 0; i--)
        {
            if (blockChildren[i].GetComponent<BaseBlock>() == null)
            {
                blockChildren.Remove(blockChildren[i]);
            }
        }

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
        Debug.Log(lowestBlocks.Count);

        // Then, find the middle point of those for the new x position
        float totalLowestX = 0f;
        foreach (BoxCollider2D block in lowestBlocks)
        {
            totalLowestX += block.transform.position.x;
        }
        float middleX = totalLowestX / lowestBlocks.Count();

        /*
        // Move all of the blocks by how much the player would be moving
        Vector2 playerNewPos = new Vector2(middleX, lowestY - (lowestBlocks[0].size.y / 2) - (col.size.y / 2));
        playerAttacher.tileParent.localPosition += (Vector3)(rb.position - playerNewPos);
        */
    }
}
