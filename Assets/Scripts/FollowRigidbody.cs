using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowRigidbody : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private Rigidbody target;

    [SerializeField]
    private bool followMovement = true;
    [SerializeField]
    private bool followRotation = false;
    [SerializeField]
    private bool verticalFollow = false;
    [SerializeField]
    private float smoothness = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 m = Vector3.zero;
    private Vector3 r = Vector3.zero;

    void FixedUpdate()
    {
        if (followMovement)
            rb.velocity = Vector3.SmoothDamp(rb.velocity,
                new Vector3(target.velocity.x, verticalFollow ? target.velocity.y : 0f, target.velocity.z),
                ref m,
                smoothness <= 0f ? 1f : smoothness * Time.fixedDeltaTime);
        if (followRotation)
            rb.angularVelocity = Vector3.SmoothDamp(rb.angularVelocity,
                target.angularVelocity,
                ref r,
                smoothness <= 0f ? 1f : smoothness * Time.fixedDeltaTime);
    }
}