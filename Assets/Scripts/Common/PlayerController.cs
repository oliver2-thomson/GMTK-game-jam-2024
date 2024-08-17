using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Basic platforming physics for the player to use

    // Public variables
    [Header("General Player Settings")]
    public int MaxHealth;
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

    private int currentHealth;

    private bool isGrounded;
    private float currentJumpTimer;
    private Vector2 currentVelocity;

    // Component references
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        // Processing inputs
        leftInput = Input.GetKey(KeyCode.LeftArrow);
        rightInput = Input.GetKey(KeyCode.RightArrow);
        if (Input.GetKey(KeyCode.Z))
        {
            jumpInput = true;
            currentJumpTimer += Time.deltaTime;
        }
        else
        {
            jumpInput = false;
            currentJumpTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        HorizontalMovement();

        GroundCheck();
        CeilingCheck();

        VerticalMovement();

        // Final velocity update
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
            if (currentJumpTimer != 0 && currentJumpTimer < MaxJumpTime)
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
        isGrounded = false;

        if (rb.velocity.y < 0)
        {
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

    public void OnHurt()
    {
        currentHealth--;
        if (currentHealth < 1)
        {
            Debug.Log("Game Over!");
        }
    }
}
