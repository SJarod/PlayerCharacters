using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private string horizontalAxis = "CameraHorizontal";
    [SerializeField]
    private string verticalAxis = "CameraVertical";

    [SerializeField]
    private float sensitivityX = 1f;
    [SerializeField]
    private float sensitivityY = 1f;

    [SerializeField]
    private Vector2 pitchRange = new Vector2(-45f, 89f);

    protected float pitch = 0f; // x axis rotation
    protected float yaw = 0f; // y axis rotation

    // camera horizontal direction
    public Vector3 cameraForward = Vector3.forward;
    public Vector3 cameraRight = Vector3.right;


    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        pitch += Input.GetAxis(verticalAxis) * sensitivityY;
        pitch = Mathf.Clamp(pitch, pitchRange.x * Mathf.Deg2Rad, pitchRange.y * Mathf.Deg2Rad);
        yaw += Input.GetAxis(horizontalAxis) * sensitivityX;

        Vector3 f = cam.transform.forward;
        cameraForward = new Vector3(f.x, 0f, f.z);
        Vector3 r = cam.transform.right;
        cameraRight = new Vector3(r.x, 0f, r.z);
    }
}