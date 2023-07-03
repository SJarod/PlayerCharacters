using UnityEngine;

public class FirstPersonCamera : CameraController
{
    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        transform.rotation = Quaternion.Euler(-pitch * Mathf.Rad2Deg, yaw * Mathf.Rad2Deg, 0f);
    }
}