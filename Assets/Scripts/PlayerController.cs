using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider playerCollider;
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

    private Vector3 inputVelocity;
    private bool bIsGrounded = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        CheckIsGrounded();

        JumpHandle();
        HorizontalMovementHandle();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(inputVelocity.x, rb.velocity.y, inputVelocity.z);
    }

    private void CheckIsGrounded()
    {
        bIsGrounded = true;
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

        if (bIsGrounded)
            inputVelocity = movementDir * maxMovementSpeed;

        if (rotateTowardsMovement && movementDir.sqrMagnitude > 0f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(movementDir),
                rotationSpeed * Time.deltaTime);
    }
}