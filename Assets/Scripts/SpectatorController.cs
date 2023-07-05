using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SpectatorController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private CameraController cameraController;


    [SerializeField]
    private string xAxis = "Horizontal";
    [SerializeField]
    private string yAxis = "Depth";
    [SerializeField]
    private string zAxis = "Vertical";


    [SerializeField]
    private float maxMovementSpeed = 5f;
    [SerializeField]
    private float acceleration = 30f;
    [SerializeField]
    private float decay = 3f;


    [SerializeField]
    private float movementCollisionDistance = 0.3f;

    private Vector3 inputVelocity;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputVelocity = cameraController.cameraRight * Input.GetAxisRaw(xAxis) +
            cameraController.cameraUp * Input.GetAxisRaw(yAxis) +
            cameraController.cameraForward * Input.GetAxisRaw(zAxis);
        inputVelocity.Normalize();
    }

    private void FixedUpdate()
    {
        if (inputVelocity.sqrMagnitude > 0f)
            rb.velocity += inputVelocity * acceleration * Time.fixedDeltaTime;
        else
            rb.velocity -= rb.velocity * decay * Time.fixedDeltaTime;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxMovementSpeed);

        // fix horizontal velocity (collision)
        RaycastHit hit;
        if (rb.SweepTest(rb.velocity, out hit, movementCollisionDistance) &&
            (!hit.rigidbody || hit.rigidbody.isKinematic))
        {
            Vector3 nv = new Vector3(Mathf.Sign(rb.velocity.x) * rb.velocity.x,
                Mathf.Sign(rb.velocity.y) * rb.velocity.y,
                Mathf.Sign(rb.velocity.z) * rb.velocity.z);
            rb.velocity += Vector3.Scale(hit.normal, nv);
        }
    }
}