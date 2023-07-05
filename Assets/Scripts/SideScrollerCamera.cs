using UnityEngine;

[ExecuteAlways]
public class SideScrollerCamera : CameraController
{
    [SerializeField]
    private Vector3 offset = Vector3.zero;


    protected override void Update()
    {
        base.Update();
        cam.transform.position = transform.position + offset;
    }
}