using UnityEngine;

public class ControllerPicker : MonoBehaviour
{
    [SerializeField]
    private string pickButton = "Pick";
    [SerializeField]
    private CameraController controller;


    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, controller.cameraForward, out hit, Mathf.Infinity))
        {
            PlayerController c;
            if (!hit.collider.gameObject.TryGetComponent<PlayerController>(out c))
                return;

            if (Input.GetButtonDown(pickButton))
            {
                Debug.Log("Taking control of " + hit.collider.name);
                c.EnableController(true);
                GetComponent<PlayerController>().EnableController(false);
            }
        }
    }
}