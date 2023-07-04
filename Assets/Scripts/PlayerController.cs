using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
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

    private Vector3 inputVelocity;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown(jumpButton))
            rb.AddForce(Vector3.up * jumpForce, useRigidbodyMass ? ForceMode.Impulse : ForceMode.VelocityChange);

        Vector3 movementDir = cameraController.cameraRight * Input.GetAxis(horizontalAxis) +
            cameraController.cameraForward * Input.GetAxis(verticalAxis);
        inputVelocity = movementDir * maxMovementSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(inputVelocity.x, rb.velocity.y, inputVelocity.z);
    }
}