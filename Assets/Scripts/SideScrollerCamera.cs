using UnityEngine;

[ExecuteAlways]
public class SideScrollerCamera : CameraController
{
    [SerializeField]
    private Vector3 offset = Vector3.zero;


    protected override void LateUpdate()
    {
        base.LateUpdate();
        cam.transform.position = transform.position + offset;
    }
}