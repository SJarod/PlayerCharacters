using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private bool verticalFollow = false;
    [SerializeField]
    private bool useInFixedUpdate = false;
    [SerializeField]
    private float smoothness = 5f;


    void LateUpdate()
    {
        if (!useInFixedUpdate)
            Follow(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (useInFixedUpdate)
            Follow(Time.fixedDeltaTime);
    }

    private void Follow(float dt)
    {
        Vector3 p = target.transform.position;
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(p.x, verticalFollow ? p.y : transform.position.y, p.z),
            smoothness * dt);
    }
}