using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : CharacterController
{
    protected Rigidbody rb;
    protected CapsuleCollider playerCollider;
    [SerializeField]
    protected CameraController cameraController;

    [SerializeField]
    protected bool bSweeptest = true;
    [SerializeField]
    protected float movementCollisionDistance = 0.1f;
    [SerializeField]
    protected bool bCreatePhysicMaterial = false;
    [SerializeField]
    protected float physicMaterialFriction = 0f;


    public bool bQueriesHitBackfaces = false;
    public bool bQueriesHitTriggers = false;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        Physics.queriesHitBackfaces = bQueriesHitBackfaces;
        Physics.queriesHitTriggers = bQueriesHitTriggers;

        if (bCreatePhysicMaterial)
            CreatePhysicMaterial(physicMaterialFriction);
    }

    protected void CreatePhysicMaterial(float friction)
    {
        Collider collider;
        if (!TryGetComponent<Collider>(out collider))
            return;

        collider.material = new PhysicMaterial("NoFriction");
        collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        collider.material.staticFriction = friction;
        collider.material.dynamicFriction = friction;
    }

    protected override void FixedUpdate()
    {
        float drag = bIsGrounded ? 1f : airDragCoefficient;

        Vector3 vh = rb.velocity;
        float vy = rb.velocity.y;

        if (inputVelocity.sqrMagnitude > 0f)
            vh += acceleration * drag * Time.fixedDeltaTime * inputVelocity;
        else
            vh -= decay * drag * Time.fixedDeltaTime * vh;
        vh = Vector3.ClampMagnitude(new Vector3(vh.x, 0f, vh.z), maxMovementSpeed);


        if (bSweeptest)
            vh = PreCollisionSweeptest(vh);


        if (bUsePlatformerPhysics)
        {
            vy -= (vy >= 0f ? jumpResistance : falloffAcceleration) * Time.fixedDeltaTime;
            vy = Mathf.Clamp(vy, -maxFalloffSpeed, Mathf.Infinity);
        }

        rb.velocity = new Vector3(vh.x, vy, vh.z);
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float vy = rb.velocity.y;
        Vector3 rel = new Vector3(collision.relativeVelocity.x, 0f, collision.relativeVelocity.z).normalized;
        rb.velocity = Vector3.Scale(-rel, new Vector3(Mathf.Abs(lastVelocity.x), 0f, Mathf.Abs(lastVelocity.z)));
        rb.velocity += Vector3.up * vy;
    }

    protected override void CheckIsGrounded()
    {
        Vector3 dir = playerCollider.transform.rotation * Vector3.up;
        Vector3 lowerSpherePos = transform.position +
            playerCollider.center -
            dir * (playerCollider.height * 0.49f - playerCollider.radius);
        bIsGrounded = Physics.SphereCast(lowerSpherePos,
            playerCollider.radius,
            Vector3.down,
            out _,
            groundCheckDistance);
    }

    protected override void JumpHandle()
    {
        if (bUsePlatformerPhysics && Input.GetButtonUp(jumpButton))
        {
            coyoteTimeCounter = 0f;
            AddGravity(ref rb, 1f, ForceMode.Impulse);
        }

        if (Input.GetButtonDown(jumpButton))
        {
            bJumpQuery = true;
            StartCoroutine(JumpMercy());
        }

        if (bJumpQuery && (bUsePlatformerPhysics ? coyoteTimeCounter > 0f : bIsGrounded))
        {
            rb.velocity -= Vector3.Scale(Vector3.up, rb.velocity);
            rb.AddForce(Vector3.up * jumpForce, bUseRigidbodyMass ? ForceMode.Impulse : ForceMode.VelocityChange);
            bJumpQuery = false;
        }
    }

    private float rVelocity = 0f;

    [SerializeField]
    private bool bLockRotation = false;
    [SerializeField]
    private bool bRotateTowardsMouse = false;
    [SerializeField]
    private bool bRotateWithAxis = false;

    protected override void HorizontalMovementHandle()
    {
        inputVelocity = cameraController.cameraRight * Input.GetAxisRaw(horizontalAxis) +
            cameraController.cameraForward * Input.GetAxisRaw(verticalAxis);
        inputVelocity.Normalize();

        if (bLockRotation)
            return;

        if (bRotateTowardsMovement && inputVelocity.sqrMagnitude > 0f)
        {
            float r = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y,
                Quaternion.LookRotation(inputVelocity).eulerAngles.y,
                ref rVelocity,
                rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, r, 0f);
        }
        else if (bRotateTowardsCamera)
        {
            // TODO : smooth
            transform.rotation = Quaternion.LookRotation(cameraController.cameraForward);
        }
        else if (bRotateTowardsMouse)
        {
            Vector3 playerScreenPosition = cameraController.cam.WorldToScreenPoint(transform.position);
            playerScreenPosition.z = 0;
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = 0;
            Vector3 dir = mouseScreenPosition - playerScreenPosition;
            dir.Normalize();
            dir.z = dir.y;
            dir.y = 0f;

            transform.rotation = Quaternion.LookRotation(dir);

        }
        else if (bRotateWithAxis)
        {
            // TODO 
        }
    }

    protected Vector3 PreCollisionSweeptest(Vector3 vh, bool bVertical = false)
    {
        // fix horizontal velocity (collision)
        RaycastHit[] hits = rb.SweepTestAll(vh.normalized, movementCollisionDistance, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                Vector3 nv;
                if (bVertical)
                    nv = new Vector3(Mathf.Sign(vh.x) * vh.x,
                        Mathf.Sign(vh.y) * vh.y,
                        Mathf.Sign(vh.z) * vh.z);
                else
                    nv = new Vector3(Mathf.Sign(vh.x) * vh.x,
                        0f,
                        Mathf.Sign(vh.z) * vh.z);
                vh += Vector3.Scale(hit.normal, nv);
            }
        }

        return vh;
    }

    public override void EnableController(bool enable)
    {
        base.EnableController(enable);
        cameraController.enabled = enable;
        cameraController.cam.depth = enable ? 1 : -1;
        cameraController.cam.enabled = enable;
        cameraController.cam.gameObject.GetComponent<AudioListener>().enabled = enable;
        inputVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerController))]
[CanEditMultipleObjects]
public class PlayerControllerEditor : Editor
{
    private PlayerController self;

    private void OnEnable()
    {
        self = (PlayerController)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Enable/Disable"))
        {
            self.EnableController(!self.enabled);
        }

        base.OnInspectorGUI();
    }
}
#endif