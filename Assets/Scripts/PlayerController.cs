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


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown(jumpButton))
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Vector3 movementDir = cameraController.cameraRight * Input.GetAxis(horizontalAxis) +
            cameraController.cameraForward * Input.GetAxis(verticalAxis);
        rb.velocity = movementDir * maxMovementSpeed;
    }

    private void FixedUpdate()
    {
        
    }
}