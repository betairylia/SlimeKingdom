using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorWithCam : eObject
{
    public eObject[] objectList;
    public string message;

    Transform camHelper;
    CameraController m_camCtrl;
    bool moveCam = false;

    // Use this for initialization
    void Start ()
    {
        camHelper = transform.Find("CameraHelper");
        if (camHelper)
        {
            moveCam = true;
            m_camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                foreach(eObject e in objectList)
                {
                    if (moveCam)
                        TriggerEvent(e, message, 1.0f);
                    else
                        TriggerEvent(e, message);
                }

                if(moveCam)
                {
                    m_camCtrl.cameraMoveTo(camHelper.position, camHelper.rotation);
                    TriggerEvent(this, "cameraRet", 3.0f);
                }
                break;
            case "cameraRet":
                m_camCtrl.cameraMoveBack();
                break;
        }
    }
}
