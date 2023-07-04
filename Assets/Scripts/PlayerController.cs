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
    private float jumpForce = 5f;

    [SerializeField]
    private bool rotateTowardsMovement = true;
    [SerializeField]
    private float rotationSpeed = 20f;

    [SerializeField]
    private float groundCheckDistance = 0.5f;

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
        if (bIsGrounded)
            rb.velocity = new Vector3(inputVelocity.x, rb.velocity.y, inputVelocity.z);
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

    private void HorizontalMovementHandle()
    {
        Vector3 movementDir = cameraController.cameraRight * Input.GetAxis(horizontalAxis) +
            cameraController.cameraForward * Input.GetAxis(verticalAxis);

        inputVelocity = movementDir * maxMovementSpeed;

        if (rotateTowardsMovement && movementDir.sqrMagnitude > 0f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(movementDir),
                rotationSpeed * Time.deltaTime);
    }
}