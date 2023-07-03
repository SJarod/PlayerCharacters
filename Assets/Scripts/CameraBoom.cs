using UnityEngine;

[ExecuteAlways]
public class CameraBoom : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonCamera cam;

    [SerializeField]
    private float springLength = 10f;


    void Start()
    {
        transform.position = transform.parent.position + cam.dir * springLength;
    }

    void Update()
    {
        RaycastHit hit;
        float d = Physics.Raycast(transform.parent.position, cam.dir, out hit, springLength) ? hit.distance : springLength;
        transform.position = transform.parent.position + cam.dir * d;
        Debug.DrawRay(transform.parent.position, cam.dir * d, Color.red);

        Quaternion lookat = Quaternion.LookRotation(transform.parent.position - transform.position);
        transform.rotation = lookat;
    }
}