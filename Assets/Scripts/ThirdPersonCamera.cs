using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
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

    private float pitch = 0f; // x axis rotation
    private float yaw = 0f; // y axis rotation

    [HideInInspector]
    public Vector3 dir = Vector3.back;


    void Start()
    {

    }

    void Update()
    {
        pitch += Input.GetAxis(verticalAxis) * sensitivityY;
        pitch = Mathf.Clamp(pitch, pitchRange.x * Mathf.Deg2Rad, pitchRange.y * Mathf.Deg2Rad);
        yaw += Input.GetAxis(horizontalAxis) * sensitivityX;

        float xaxis = Mathf.Cos(yaw);
        float zaxis = Mathf.Sin(yaw);
        float yaxis = pitch;

        float x = xaxis * Mathf.Cos(yaxis);
        float y = Mathf.Sin(yaxis);
        float z = zaxis * Mathf.Cos(yaxis);

        dir = new Vector3(x, y, z);
    }
}