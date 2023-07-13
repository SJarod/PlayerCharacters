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
    protected bool useRigidbodyMass = true;
    [SerializeField]
    protected float maxMovementSpeed = 5f;
    [SerializeField]
    protected float acceleration = 30f;
    [SerializeField]
    protected float decay = 3f;
    [SerializeField]
    protected float jumpForce = 5f;

    [SerializeField]
    protected bool rotateTowardsMovement = true;
    [SerializeField]
    protected bool rotateTowardsCamera = false;
    [SerializeField]
    protected float rotationSpeed = 0.1f;

    [SerializeField]
    protected float groundCheckDistance = 0.5f;

    [SerializeField]
    protected float movementCollisionDistance = 0.3f;

    [SerializeField]
    protected float airDragCoefficient = 0.3f;

    protected Vector3 inputVelocity = Vector3.zero;
    protected Vector3 lastVelocity = Vector3.zero;
    protected bool bIsGrounded = false;

    [HideInInspector]
    public Vector3 lastInputVelocity = Vector3.zero;


    protected virtual void Update()
    {
        CheckIsGrounded();

        JumpHandle();
        HorizontalMovementHandle();

        if (inputVelocity.sqrMagnitude > 0f)
            lastInputVelocity = inputVelocity.normalized;
    }

    protected abstract void FixedUpdate();

    protected abstract void CheckIsGrounded();

    protected abstract void JumpHandle();

    protected abstract void HorizontalMovementHandle();

    public virtual void EnableController(bool enable)
    {
        enabled = enable;
    }
}