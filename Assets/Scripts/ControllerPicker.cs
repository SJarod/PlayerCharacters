using UnityEngine;

public class ControllerPicker : MonoBehaviour
{
    [SerializeField]
    private string pickButton = "Pick";
    private CameraController cameraController;


    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraController.cameraForward, out hit, Mathf.Infinity))
        {
            PlayerController c;
            if (!hit.collider.gameObject.TryGetComponent<PlayerController>(out c))
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