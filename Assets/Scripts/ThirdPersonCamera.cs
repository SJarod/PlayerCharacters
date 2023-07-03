using UnityEngine;

public class ThirdPersonCamera : CameraController
{
    [HideInInspector]
    public Vector3 dir = Vector3.back;


    protected override void Start()
    {

    }

    protected override void Update()
    {
        base.Update();

        float xaxis = Mathf.Cos(yaw);
        float zaxis = Mathf.Sin(yaw);
        float yaxis = pitch;

        float x = xaxis * Mathf.Cos(yaxis);
        float y = Mathf.Sin(yaxis);
        float z = zaxis * Mathf.Cos(yaxis);

        dir = new Vector3(x, y, z);
    }
}