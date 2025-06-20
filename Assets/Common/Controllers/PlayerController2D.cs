using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController2D : CharacterController
{
    protected Rigidbody2D rb;
    protected CapsuleCollider2D playerCollider;


    public bool bQueriesStartInColliders = false;
    public bool bQueriesHitTriggers = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        Physics2D.queriesStartInColliders = bQueriesStartInColliders;
        Physics2D.queriesHitTriggers = bQueriesHitTriggers;
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

        if (bUsePlatformerPhysics)
        {
            vy -= (vy >= 0f ? jumpResistance : falloffAcceleration) * Time.fixedDeltaTime;
            vy = Mathf.Clamp(vy, -maxFalloffSpeed, Mathf.Infinity);
        }

        rb.velocity = new Vector2(vx, vy);
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float vy = rb.velocity.y;
        rb.velocity = new Vector2(-collision.relativeVelocity.x, 0f).normalized * Mathf.Abs(lastVelocity.x);
        rb.velocity += Vector2.up * vy;
    }

    protected override void CheckIsGrounded()
    {
        bIsGrounded = Physics2D.CircleCast(transform.position, playerCollider.size.x, Vector2.down, groundCheckDistance);
    }

    protected override void HorizontalMovementHandle()
    {
        inputVelocity = Vector2.right * Input.GetAxisRaw(horizontalAxis);
        inputVelocity.Normalize();
    }

    protected override void JumpHandle()
    {
        if (bUsePlatformerPhysics && Input.GetButtonUp(jumpButton))
        {
            coyoteTimeCounter = 0f;
            AddGravity2D(ref rb, 1f, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown(jumpButton))
        {
            bJumpQuerry = true;
            StartCoroutine(JumpMercy());
        }

        if (bJumpQuerry && (bUsePlatformerPhysics ? coyoteTimeCounter > 0f : bIsGrounded))
        {
            rb.velocity -= Vector2.Scale(Vector2.up, rb.velocity);
            rb.AddForce(Vector2.up * jumpForce * (bUseRigidbodyMass ? rb.mass : 1f), ForceMode2D.Impulse);
            bJumpQuerry = false;
        }
    }
}