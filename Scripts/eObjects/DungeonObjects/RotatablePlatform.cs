using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatablePlatform : eObject
{
    CameraController camCtrl;
    float target = 0;

    // Use this for initialization
    void Start ()
    {
        camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update ()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, target, 0), 120.0f * Time.deltaTime);
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "rotate1":

                camCtrl.cameraMoveTo(transform.Find("CameraHelper").position, transform.Find("CameraHelper").rotation);
                TriggerEvent(this, "CameraRet", 4.0f);
                TriggerEvent(this, "_rotate1", 1.0f);

                break;
            case "_rotate1":

                if (target != 120)
                    target = 120;
                else
                    target = 0;

                break;
            case "rotate2":

                camCtrl.cameraMoveTo(transform.Find("CameraHelper").position, transform.Find("CameraHelper").rotation);
                TriggerEvent(this, "CameraRet", 4.0f);
                TriggerEvent(this, "_rotate2", 1.0f);

                break;
            case "_rotate2":

                if (target != -120)
                    target = -120;
                else
                    target = 120;

                break;
            case "CameraRet":
                camCtrl.cameraMoveBack();
                break;
        }
    }
}
