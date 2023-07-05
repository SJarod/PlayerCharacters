using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    [SerializeField]
    private CameraController cameraController;


    [SerializeField]
    private string horizontalAxis = "Horizontal";
    [SerializeField]
    private string verticalAxis = "Vertical";
    [SerializeField]
    private string jumpButton = "Jump";


    [SerializeField]
    private bool useRigidbodyMass = true;
    [SerializeField]
    private float maxMovementSpeed = 5f;
    [SerializeField]
    private float acceleration = 30f;
    [SerializeField]
    private float decay = 3f;
    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private bool rotateTowardsMovement = true;
    [SerializeField]
    private bool rotateTowardsCamera = false;
    [SerializeField]
    private float rotationSpeed = 0.1f;

    [SerializeField]
    private float groundCheckDistance = 0.5f;

    [SerializeField]
    private float movementCollisionDistance = 0.3f;

    [SerializeField]
    private float airDragCoefficient = 0.3f;

    private Vector3 inputVelocity;
    private bool bIsGrounded = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        CheckIsGrounded();

        JumpHandle();
        HorizontalMovementHandle();
    }

    private void FixedUpdate()
    {
        float yVelocity = rb.velocity.y;
        float drag = bIsGrounded ? 1f : airDragCoefficient;

        if (inputVelocity.sqrMagnitude > 0f)
            rb.velocity += inputVelocity * acceleration * drag * Time.fixedDeltaTime;
        else
            rb.velocity -= rb.velocity * decay * drag * Time.fixedDeltaTime;
        rb.velocity = Vector3.ClampMagnitude(new Vector3(rb.velocity.x, 0f, rb.velocity.z), maxMovementSpeed);

        // fix horizontal velocity (collision)
        RaycastHit hit;
        if (rb.SweepTest(rb.velocity, out hit, movementCollisionDistance) &&
            (!hit.rigidbody || hit.rigidbody.isKinematic))
        {
            Vector3 hn = new Vector3(hit.normal.x, 0f, hit.normal.z);
            Vector3 nv = new Vector3(Mathf.Sign(rb.velocity.x) * rb.velocity.x,
                0f,
                Mathf.Sign(rb.velocity.z) * rb.velocity.z);
            rb.velocity += Vector3.Scale(hn.normalized, nv);
        }

        rb.velocity = new Vector3(rb.velocity.x, yVelocity, rb.velocity.z);
    }

    private void CheckIsGrounded()
    {
        CapsuleCollider cap = playerCollider;
        Vector3 dir = cap.transform.rotation * Vector3.up;
        Vector3 start = transform.position + cap.center + dir * (cap.height * 0.5f - cap.radius);
        Vector3 end = transform.position + cap.center - dir * (cap.height * 0.49f - cap.radius);
        RaycastHit hit;
        bIsGrounded = Physics.CapsuleCast(start, end, cap.radius, Vector3.down, out hit, groundCheckDistance);
    }

    private void JumpHandle()
    {
        if (!bIsGrounded)
            return;

        if (Input.GetButtonDown(jumpButton))
        {
            rb.velocity -= Vector3.Scale(Vector3.up, rb.velocity);
            rb.AddForce(Vector3.up * jumpForce, useRigidbodyMass ? ForceMode.Impulse : ForceMode.VelocityChange);
        }
    }

    private float rVelocity = 0f;

    private void HorizontalMovementHandle()
    {
        inputVelocity = cameraController.cameraRight * Input.GetAxisRaw(horizontalAxis) +
            cameraController.cameraForward * Input.GetAxisRaw(verticalAxis);
        inputVelocity.Normalize();

        if (rotateTowardsMovement && inputVelocity.sqrMagnitude > 0f)
        {
            float r = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y,
                Quaternion.LookRotation(inputVelocity).eulerAngles.y,
                ref rVelocity,
                rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, r, 0f);
        }
        else if (rotateTowardsCamera)
        {
            transform.rotation = Quaternion.LookRotation(cameraController.cameraForward);
        }
    }
}