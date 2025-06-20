using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;

    [SerializeField]
    private string horizontalAxis = "CameraHorizontal";
    [SerializeField]
    private string verticalAxis = "CameraVertical";

    [SerializeField]
    private float sensitivityX = 1f;
    [SerializeField]
    private float sensitivityY = 1f;
    [SerializeField]
    private bool invertY = false;
    [SerializeField]
    private float smoothness = 0.15f;

    [SerializeField]
    private Vector2 pitchRange = new Vector2(-45f, 89f);

    // x axis rotation
    private float pp = 0f; // next pitch
    protected float pitch = 0f;
    // y axis rotation
    private float yy = 0f; // next yaw
    protected float yaw = 0f;

    // camera horizontal direction
    public Vector3 cameraRight = Vector3.right;
    public Vector3 cameraUp = Vector3.up;
    public Vector3 cameraForward = Vector3.forward;


    protected virtual void Start()
    {

    }

    private float pVelocity = 0f;
    private float yVelocity = 0f;

    protected virtual void LateUpdate()
    {
        Vector2 v = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));

        pp += v.y * sensitivityY * (invertY ? 1f : -1f);
        pp = Mathf.Clamp(pp, pitchRange.x * Mathf.Deg2Rad, pitchRange.y * Mathf.Deg2Rad);
        yy += v.x * sensitivityX;

        if (pitch == pp)
            pVelocity = 0f;
        if (yaw == yy)
            yVelocity = 0f;
        pitch = Mathf.SmoothDamp(pitch, pp, ref pVelocity, smoothness);
        yaw = Mathf.SmoothDamp(yaw, yy, ref yVelocity, smoothness);



        Vector3 r = cam.transform.right;
        cameraRight = new Vector3(r.x, 0f, r.z).normalized;
        Vector3 u = cam.transform.up;
        cameraUp = new Vector3(0f, u.y, 0f).normalized;
        Vector3 f = cam.transform.forward;
        cameraForward = new Vector3(f.x, 0f, f.z).normalized;
    }
}