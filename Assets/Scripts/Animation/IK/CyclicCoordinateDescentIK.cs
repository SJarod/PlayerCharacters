using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclicCoordinateDescentIK : MonoBehaviour
{
    // joints/bones
    private List<Joint> joints = new List<Joint>();

    // iterative inverse kinematic max iteration count
    public int maxIteration = 10;


    // object for end effector to reach
    public GameObject targetObject;


    // Start is called before the first frame update
    void Start()
    {
        Joint[] j = GetComponentsInChildren<Joint>();
        joints.AddRange(j);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maxIteration; ++i)
        {
            Vector3 targetPos = targetObject.transform.position;

            for (int j = joints.Count - 1; j >= 0; --j)
            {
                Vector3 endEffector = joints[joints.Count - 1].transform.position;

                Vector3 itJoint = joints[j].transform.position;

                // vector from itereated joint to end effector
                Vector3 ei = (endEffector - itJoint).normalized;
                // vector from iterated joint to target
                Vector3 ti = (targetPos - itJoint).normalized;


                float theta = Mathf.Deg2Rad * Vector3.Angle(ei, ti);
                Vector3 axis = Vector3.Cross(ei, ti).normalized;


                // quaternion = a + bi + cj + dk;
                // a = c = cos(theta / 2)
                // b = c = d = sin(theta / 2)
                // ii = jj = kk = -1
                // ij = k
                // jk = i
                // ki = j
                float c = Mathf.Cos(theta / 2.0f);
                float s = Mathf.Sin(theta / 2.0f);
                Vector3 saxis = s * axis;
                joints[j].transform.rotation *= new Quaternion(saxis.x, saxis.y, saxis.z, c);
            }
        }
    }
}
