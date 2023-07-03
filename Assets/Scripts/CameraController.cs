using UnityEngine;

public class CameraController : MonoBehaviour
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

    protected float pitch = 0f; // x axis rotation
    protected float yaw = 0f; // y axis rotation


    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        pitch += Input.GetAxis(verticalAxis) * sensitivityY;
        pitch = Mathf.Clamp(pitch, pitchRange.x * Mathf.Deg2Rad, pitchRange.y * Mathf.Deg2Rad);
        yaw += Input.GetAxis(horizontalAxis) * sensitivityX;
    }
}