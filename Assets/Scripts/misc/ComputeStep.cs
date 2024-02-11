using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeStep : MonoBehaviour
{
    public GameObject targetToMove;

    private float timeStamp = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeStamp < 0.2f)
            return;

        timeStamp = Time.time;

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 99.0f);

        targetToMove.transform.position = hit.point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetToMove.transform.position, 0.2f);
    }
}
