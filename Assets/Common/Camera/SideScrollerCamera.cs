using UnityEngine;

[ExecuteAlways]
public class SideScrollerCamera : CameraController
{
    [SerializeField]
    private Vector3 offsetDir = -Vector3.forward;
    [SerializeField]
    private float cameraDistance = 10f;

    [SerializeField]
    private bool bSyncOffsetDirWithPosition = false;

    private void Awake()
    {
        if (bSyncOffsetDirWithPosition)
            offsetDir = transform.position;

        offsetDir.Normalize();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        cam.transform.position = transform.position + offsetDir * cameraDistance;
    }
}