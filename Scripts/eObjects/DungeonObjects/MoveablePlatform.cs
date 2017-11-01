using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatform : eObject
{
    public bool isVisibleFirst;
    public float debugtmp_UpDist, upSpeed;

    CameraController camCtrl;

    bool begin = false;

	// Use this for initialization
	void Start ()
    {
        if (gameObject.GetComponent<Renderer>())
        {
            gameObject.GetComponent<Renderer>().enabled = isVisibleFirst;
        }
        transform.Translate(Vector3.down * debugtmp_UpDist, Space.Self);

        camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(begin)
        {
            transform.Translate(Vector3.up * Time.deltaTime * upSpeed);
            if(transform.localPosition.y >= 0)
            {
                begin = false;
            }
        }
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                if (gameObject.GetComponent<Renderer>())
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
                begin = true;
                camCtrl.cameraMoveTo(transform.Find("CameraHelper").position, transform.Find("CameraHelper").rotation);
                TriggerEvent(this, "CameraRet", 3.0f);
                break;

            case "CameraRet":
                camCtrl.cameraMoveBack();
                break;
        }
    }
}
