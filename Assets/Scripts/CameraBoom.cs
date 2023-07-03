using UnityEngine;

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
        Boom(cam.dir);
        LookAtParent();
    }

    private void Boom(Vector3 dir)
    {
        RaycastHit hit;
        float d = Physics.Raycast(transform.parent.position, dir, out hit, springLength) ? hit.distance : springLength;
        transform.position = transform.parent.position + dir * d;
        Debug.DrawRay(transform.parent.position, dir * d, Color.red);
    }

    private void LookAtParent()
    {
        Quaternion lookat = Quaternion.LookRotation(transform.parent.position - transform.position);
        transform.rotation = lookat;
    }

    private void OnDrawGizmos()
    {
        Boom(Vector3.back);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.parent.position);
        Gizmos.color = Color.white;

        LookAtParent();
    }
}