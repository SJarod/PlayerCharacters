using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController2D : Controller
{
    protected Rigidbody2D rb;
    protected CapsuleCollider2D playerCollider;


    [SerializeField]
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        Physics2D.queriesStartInColliders = false;
        Physics2D.queriesHitTriggers = false;
    }

    protected override void FixedUpdate()
    {
        float drag = bIsGrounded ? 1f : airDragCoefficient;

        float vx = rb.velocity.x;
        float vy = rb.velocity.y;

        if (inputVelocity.sqrMagnitude > 0f)
            vx += inputVelocity.x * acceleration * drag * Time.fixedDeltaTime;
        else
            vx -= vx * decay * drag * Time.fixedDeltaTime;
        vx = Mathf.Clamp(vx, -maxMovementSpeed, maxMovementSpeed);

        rb.velocity = new Vector2(vx, vy);
    }

    protected override void CheckIsGrounded()
    {
        bIsGrounded = Physics2D.CircleCast(transform.position, playerCollider.size.x, Vector2.down, groundCheckDistance);
        if (bIsGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    protected override void HorizontalMovementHandle()
    {
        inputVelocity = Vector2.right * Input.GetAxisRaw(horizontalAxis);
        inputVelocity.Normalize();
    }

    protected override void JumpHandle()
    {
        if (!bIsGrounded)
            return;

        if (Input.GetButtonDown(jumpButton))
        {
            rb.velocity -= Vector2.Scale(Vector2.up, rb.velocity);
            rb.AddForce(Vector2.up * jumpForce * (useRigidbodyMass ? rb.mass : 1f), ForceMode2D.Impulse);
        }
    }
}