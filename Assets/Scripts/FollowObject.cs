using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private bool followMovement = true;
    [SerializeField]
    private bool followRotation = false;
    [SerializeField]
    private bool verticalFollow = false;
    [SerializeField]
    private bool useInLateUpdate = false;
    [SerializeField]
    private bool useInFixedUpdate = false;
    [SerializeField]
    private float smoothness = 5f;


    void Update()
    {
        if (!useInFixedUpdate && !useInLateUpdate)
            Follow(Time.deltaTime);
    }

    void LateUpdate()
    {
        if (!useInFixedUpdate && useInLateUpdate)
            Follow(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (useInFixedUpdate)
            Follow(Time.fixedDeltaTime);
    }

    private void Follow(float dt)
    {
        if (followMovement)
        {
            Vector3 p = target.transform.position;
            Vector3 t = new Vector3(p.x, verticalFollow ? p.y : transform.position.y, p.z);
            transform.position = Vector3.Lerp(transform.position, t, smoothness <= 0f ? 1f : smoothness * dt);
        }
        if (followRotation)
        {
            Vector3 f = target.transform.forward;
            Vector3 ff = new Vector3(f.x, verticalFollow ? f.y : transform.forward.y, f.z);
            Quaternion t = Quaternion.LookRotation(ff.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, t, smoothness <= 0f ? 1f : smoothness * dt);
        }
    }
}