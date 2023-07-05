using UnityEngine;

public class ThirdPersonCamera : CameraController
{
    [HideInInspector]
    public Vector3 dir = Vector3.back;


    protected override void Start()
    {

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        float xaxis = Mathf.Sin(yaw);
        float zaxis = Mathf.Cos(yaw);
        float yaxis = pitch;

        float x = xaxis * Mathf.Cos(yaxis);
        float y = Mathf.Sin(yaxis);
        float z = zaxis * Mathf.Cos(yaxis);

        dir = new Vector3(x, y, z);
    }
}