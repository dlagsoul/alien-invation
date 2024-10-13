using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool activeFP;

    public Transform posTP;
    public Transform posFP;

    //Camara TP
    public float rotSpeed;
    public float rotMinFP, rotMaxFP, rotMinTP, rotMaxTP;
    float mouseX, mouseY;
    public Transform target, player;
    
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InitCam();
    }

    public void Cam()
    {
        mouseX += rotSpeed * Input.GetAxis("Mouse X");
        mouseY -= rotSpeed * Input.GetAxis("Mouse Y");
        
        float rotMin = activeFP ? rotMinFP : rotMinTP;
        float rotMax = activeFP ? rotMaxFP : rotMaxTP;
        
        mouseY = Mathf.Clamp(mouseY, rotMin, rotMax);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0.0f);
        player.rotation = Quaternion.Euler(0.0f, mouseX, 0.0f);
    }

    void InitCam()
    {
        if (activeFP == false)
        {
            transform.position = posFP.position;
        }
        else if(activeFP == true)
        {
            transform.position = posTP.position;
            transform.LookAt(player);
        }
    }

    void LateUpdate()
    {
        Cam();

        // if (activeFP == false && Input.GetKeyDown(KeyCode.Tab))
        // {
        //     activeFP = true;
        //     transform.position = posFP.position;
        // }
        // else if(activeFP == true && Input.GetKeyDown(KeyCode.Tab))
        // {
        //     activeFP = false;
        //     transform.position = posTP.position;
        //     transform.LookAt(player);
        // }
    }
}
