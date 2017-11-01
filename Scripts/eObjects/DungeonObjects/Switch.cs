using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : eObject
{
    public Material debugtmp_newMaterial;
    public eObject targetObject;
    public string message = "Activate";

    Transform camHelper;
    CameraController m_camCtrl;

    bool turnedOn = false;
    bool moveCam = false;

    // Use this for initialization
    void Start()
    {
        camHelper = transform.Find("CameraHelper");
        if (camHelper)
        {
            moveCam = true;
            m_camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        Debug.Log(signal[0]);

        switch (signal[0])
        {
            case "damage":
            case "damageToEnemy":
                if(source.GetState("IgnoreSwitch") > 0)
                {
                    break;
                }
                if(!turnedOn)
                {
                    turnedOn = true;
                    GetComponent<Renderer>().material = debugtmp_newMaterial;

                    if(!moveCam)
                    {
                        TriggerEvent(targetObject, message);
                    }
                    else
                    {
                        TriggerEvent(targetObject, message, 1.0f);
                        m_camCtrl.cameraMoveTo(camHelper.position, camHelper.rotation);
                        TriggerEvent(this, "cameraRet", 3.0f);
                    }
                }
                break;

            case "cameraRet":
                m_camCtrl.cameraMoveBack();
                break;
            
            //todo: camera movement
        }
    }
}
