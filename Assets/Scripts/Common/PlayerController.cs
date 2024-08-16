using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Basic platforming physics for the player to use

    [Header("Player Physics")]
    public float MoveSpeed;
    public float MoveAccel;
    public float MoveMidairAccel;
    [Space(10)]
    public float JumpSpeed;
    public float Gravity;

    private bool leftInput;
    private bool rightInput;
    private bool jumpInput;

    private bool isGrounded;
    private Vector2 currentVelocity;

    // Component references
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        leftInput = Input.GetKey(KeyCode.LeftArrow);
        rightInput = Input.GetKey(KeyCode.RightArrow);
        jumpInput = Input.GetKey(KeyCode.Z);
    }

    private void FixedUpdate()
    {
        // Horizontal movement
        if (isGrounded)
        {
            currentVelocity.x = Mathf.MoveTowards(rb.velocity.x, MoveSpeed * (leftInput ? -1 : rightInput ? 1 : 0), MoveAccel);
        }
        else
        {
            currentVelocity.x = Mathf.MoveTowards(rb.velocity.x, MoveSpeed * (leftInput ? -1 : rightInput ? 1 : 0), MoveMidairAccel);
        }

        // Vertical movement
        isGrounded = false;

        // Ground check
        RaycastHit2D[] groundCheck = Physics2D.BoxCastAll(rb.position, col.size, Mathf.Round(transform.eulerAngles.z), Vector2.down);
        foreach (RaycastHit2D hit in groundCheck)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (Mathf.Floor(hit.distance * 10) / 10 <= 0 && hit.normal == Vector2.up)
                {
                    rb.position = new Vector2(rb.position.x, hit.point.y + col.size.y / 2);
                    isGrounded = true;
                    break;
                }
            }
        }

        // Ceiling check
        RaycastHit2D[] ceilingCheck = Physics2D.BoxCastAll(rb.position, col.size, Mathf.Round(transform.eulerAngles.z), Vector2.up);
        foreach (RaycastHit2D hit in ceilingCheck)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (Mathf.Floor(hit.distance * 10) / 10 <= 0 && hit.normal == Vector2.down)
                {
                    rb.position = new Vector2(rb.position.x, hit.point.y - col.size.y / 2 - 0.1f);
                    currentVelocity.y = -Gravity;
                    break;
                }
            }
        }

        if (!isGrounded)
        {
            currentVelocity.y -= Gravity;
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

        // Final velocity update
        rb.velocity = currentVelocity;
    }
}
