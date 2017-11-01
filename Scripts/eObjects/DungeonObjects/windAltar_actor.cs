using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windAltar_actor : eObject
{
    public GameObject[] windObj;
    public float switchOffTime;

    Transform cameraPos;
    CameraController camCtrl;

    Vector3 posTmp;
    Quaternion rotTmp;

    bool turnedOn = false;

	// Use this for initialization
	void Start ()
	{
        foreach (GameObject wind in windObj)
        {
            wind.SetActive(false);
        }

        cameraPos = transform.Find("CameraHelper");
        camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
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
            case "damage":
            case "damageToEnemy":
                if(!turnedOn)
                {
                    turnedOn = true;
                    TriggerEvent(this, "Activate", 1.0f);
                    camCtrl.cameraMoveTo(cameraPos.position, cameraPos.rotation);
                    TriggerEvent(this, "cameraRet", 2.5f);
                }
                break;

            case "timeOver":
                TriggerEvent(this, "Disable", 1.0f);
                camCtrl.cameraMoveTo(cameraPos.position, cameraPos.rotation);
                TriggerEvent(this, "cameraRet", 2.5f);
                TriggerEvent(this, "finish", 2.5f);
                break;

            case "Disable":
                foreach (GameObject wind in windObj)
                {
                    wind.SetActive(false);
                }
                break;

            case "Activate":
                foreach (GameObject wind in windObj)
                {
                    wind.SetActive(true);
                }

                if (switchOffTime > 0)
                {
                    TriggerEvent(this, "timeOver", switchOffTime);
                    GameObject.Find("UIController").GetComponent<UIController>().timer = switchOffTime;
                }
                break;

            case "cameraRet":
                camCtrl.cameraMoveBack();
                break;

            case "finish":
                turnedOn = false;
                break;
        }
    }
}
