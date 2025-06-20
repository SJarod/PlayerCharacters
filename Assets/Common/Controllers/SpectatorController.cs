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


        if (bSweeptest)
            rb.velocity = PreCollisionSweeptest(rb.velocity, true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpectatorController))]
[CanEditMultipleObjects]
public class SpectatorControllerEditor : PlayerControllerEditor
{

}
#endif