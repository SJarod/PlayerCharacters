using UnityEngine;

public class ControllerPicker : MonoBehaviour
{
    [SerializeField]
    private string pickButton = "Pick";
    [SerializeField]
    private CameraController cameraController;


    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraController.cam.transform.position,
            cameraController.cam.transform.forward,
            out hit,
            Mathf.Infinity))
        {
            PlayerController c;
            if (!hit.collider.gameObject.TryGetComponent<PlayerController>(out c))
                return;

            if (c == GetComponent<PlayerController>())
                return;

            if (Input.GetButtonDown(pickButton))
            {
                Debug.Log("Taking control of " + hit.collider.name);
                c.EnableController(true);
                c.GetComponent<ControllerPicker>().enabled = true;
                GetComponent<PlayerController>().EnableController(false);
                enabled = false;
            }
        }
    }
}