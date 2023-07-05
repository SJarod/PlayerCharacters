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
    private bool verticalFollow = false;
    [SerializeField]
    private float smoothness = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 m = Vector3.zero;

    void FixedUpdate()
    {
        if (followMovement)
        {
            if (rb.transform.position == target.transform.position)
                m = Vector3.zero;

            rb.MovePosition(Vector3.SmoothDamp(rb.transform.position,
                new Vector3(target.transform.position.x,
                verticalFollow ? target.transform.position.y : 0f,
                target.transform.position.z),
                ref m,
                smoothness <= 0f ? 1f : smoothness * Time.fixedDeltaTime));
        }
    }
}