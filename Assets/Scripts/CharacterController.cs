using System.Collections;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [SerializeField]
    protected string horizontalAxis = "Horizontal";
    [SerializeField]
    protected string verticalAxis = "Vertical";
    [SerializeField]
    protected string jumpButton = "Jump";


    [SerializeField]
    protected bool bUseRigidbodyMass = true;
    [SerializeField]
    protected bool bUsePlatformerPhysics = true;
    [SerializeField]
    protected float maxMovementSpeed = 10f;
    [SerializeField]
    protected float acceleration = 99f;
    [SerializeField]
    protected float decay = 8f;
    [SerializeField]
    protected float jumpForce = 25f;

    [SerializeField]
    protected bool bRotateTowardsMovement = true;
    [SerializeField]
    protected bool bRotateTowardsCamera = false;
    [SerializeField]
    protected float rotationSpeed = 0.1f;

    [SerializeField]
    protected float groundCheckDistance = 0.1f;

    [SerializeField]
    protected float movementCollisionDistance = 0.3f;

    [SerializeField]
    protected float airDragCoefficient = 0.2f;
    [SerializeField]
    protected float jumpResistance = 49f;
    [SerializeField]
    protected float falloffAcceleration = 51f;
    [SerializeField]
    protected float maxFalloffSpeed = 50f;
    [SerializeField]
    protected float gravityMultiplier = 1.1f;

    [SerializeField]
    protected float jumpMercy = 0.1f;
    protected bool bJumpQuerry = false;

    [SerializeField]
    protected float coyoteTime = 0.1f;
    protected float coyoteTimeCounter = 0f;

    [SerializeField]
    private float adaptativeJumpSpeedThreshold = 15f;


    protected Vector3 inputVelocity = Vector3.zero;
    protected Vector3 lastVelocity = Vector3.zero;
    protected bool bIsGrounded = false;

    [HideInInspector]
    public Vector3 lastInputVelocity = Vector3.zero;


    protected virtual void Update()
    {
        CheckIsGrounded();
        if (bUsePlatformerPhysics)
        {
            if (bIsGrounded)
                coyoteTimeCounter = coyoteTime;
            else
                coyoteTimeCounter -= Time.deltaTime;
        }

        JumpHandle();
        HorizontalMovementHandle();

        if (inputVelocity.sqrMagnitude > 0f)
            lastInputVelocity = inputVelocity.normalized;
    }

    protected abstract void FixedUpdate();

    protected abstract void CheckIsGrounded();

    protected IEnumerator JumpMercy()
    {
        yield return new WaitForSeconds(jumpMercy);
        bJumpQuerry = false;
    }

    protected abstract void JumpHandle();

    protected void AddGravity(ref Rigidbody rb, float multiplier, ForceMode mode, bool force = false)
    {
        if (rb.velocity.y > adaptativeJumpSpeedThreshold || force)
            rb.AddForce(Vector3.up * Physics.gravity.y * multiplier, mode);
    }

    protected void AddGravity2D(ref Rigidbody2D rb, float multiplier, ForceMode2D mode, bool force = false)
    {
        if (rb.velocity.y > adaptativeJumpSpeedThreshold || force)
            rb.AddForce(Vector2.up * Physics2D.gravity.y * multiplier, mode);
    }

    protected abstract void HorizontalMovementHandle();

    public virtual void EnableController(bool enable)
    {
        enabled = enable;
    }
}