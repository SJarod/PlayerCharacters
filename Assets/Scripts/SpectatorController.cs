using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SpectatorController : PlayerController
{
    [SerializeField]
    private string depthAxis = "Depth";


    protected override void Update()
    {
        inputVelocity = cameraController.cameraRight * Input.GetAxisRaw(horizontalAxis) +
            cameraController.cameraUp * Input.GetAxisRaw(depthAxis) +
            cameraController.cameraForward * Input.GetAxisRaw(verticalAxis);
        inputVelocity.Normalize();
    }

    protected override void FixedUpdate()
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

#if UNITY_EDITOR
[CustomEditor(typeof(SpectatorController))]
public class SpectatorControllerEditor : PlayerControllerEditor
{

}
#endif