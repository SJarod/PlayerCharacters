using UnityEngine;

public class CameraBoom : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonCamera controller;

    [SerializeField]
    private float springLength = 10f;
    [SerializeField]
    private float boomSpeed = 1f;


    void Start()
    {
        transform.position = transform.parent.position + controller.dir * springLength;
    }

    void Update()
    {
        Boom(controller.dir);
        LookAtParent();
    }

    private float d = 0f;
    private float dVelocity = 0f;

    private void Boom(Vector3 dir)
    {
        RaycastHit[] hits = new RaycastHit[3];
        int collisions = Physics.RaycastNonAlloc(transform.parent.position, dir, hits, springLength);
        for (int i = 0; i < collisions; ++i) // collision
        {
            bool isCharacter = hits[i].collider.gameObject.TryGetComponent<PlayerController>(out _);

            if (!isCharacter && hits[i].distance <= d)
                d = hits[i].distance;
            else
                d = Mathf.SmoothDamp(d, springLength, ref dVelocity, boomSpeed);
        }
        if (collisions == 0) // no collision
        {
            d = Mathf.SmoothDamp(d, springLength, ref dVelocity, boomSpeed);
        }
        transform.position = transform.parent.position + dir * d;

        Debug.DrawRay(transform.parent.position, dir * d, Color.red);
    }

    private void LookAtParent()
    {
        Vector3 at = transform.parent.position - transform.position;
        if (at == Vector3.zero)
            return;

        Quaternion lookat = Quaternion.LookRotation(at);
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