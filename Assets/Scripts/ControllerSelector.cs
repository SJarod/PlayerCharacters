using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ControllerSelector : MonoBehaviour
{
    private PlayerController spec;
    private PlayerController fps;
    private PlayerController tps;
    private PlayerController ss;


    private void Start()
    {
        spec = GameObject.Find("SpectatorCharacter").GetComponent<PlayerController>();
        fps = GameObject.Find("FirstPersonCharacter").GetComponent<PlayerController>();
        tps = GameObject.Find("ThirdPersonCharacter").GetComponentInChildren<PlayerController>();
        ss = GameObject.Find("SideScrollerCharacter").GetComponentInChildren<PlayerController>();
    }

    void Update()
    {
        Action allFalse = () => {
            spec.EnableController(false);
            fps.EnableController(false);
            tps.EnableController(false);
            ss.EnableController(false);
        };

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            allFalse();
            spec.EnableController(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            allFalse();
            fps.EnableController(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            allFalse();
            tps.EnableController(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            allFalse();
            ss.EnableController(true);
        }
    }
}