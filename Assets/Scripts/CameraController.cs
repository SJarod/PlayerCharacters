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
    private bool invertY = false;
    [SerializeField]
    private float smoothnessSpeed = 5f;

    [SerializeField]
    private Vector2 pitchRange = new Vector2(-45f, 89f);

    // x axis rotation
    private float actualPitch = 0f;
    protected float pitch = 0f;
    // y axis rotation
    private float actualYaw = 0f;
    protected float yaw = 0f;

    // camera horizontal direction
    public Vector3 cameraForward = Vector3.forward;
    public Vector3 cameraRight = Vector3.right;


    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        Vector2 v = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));

        actualPitch += v.y * sensitivityY * (invertY ? 1f : -1f);
        actualPitch = Mathf.Clamp(actualPitch, pitchRange.x * Mathf.Deg2Rad, pitchRange.y * Mathf.Deg2Rad);
        actualYaw += v.x * sensitivityX;

        float s = smoothnessSpeed != 0f ? smoothnessSpeed * Time.deltaTime : 1f;

        pitch = Mathf.Lerp(pitch, actualPitch, s);
        yaw = Mathf.Lerp(yaw, actualYaw, s);



        Vector3 f = cam.transform.forward;
        cameraForward = (new Vector3(f.x, 0f, f.z)).normalized;
        Vector3 r = cam.transform.right;
        cameraRight = (new Vector3(r.x, 0f, r.z)).normalized;
    }
}