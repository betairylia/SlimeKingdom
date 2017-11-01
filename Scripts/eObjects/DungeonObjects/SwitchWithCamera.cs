using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class SwitchWithCamera : eObject
{
    public Material debugtmp_newMaterial;
    public eObject targetObject;
    public string message = "Activate";
    public Transform cameraPos;

    bool turnedOn = false;

    CameraController m_camCtrl;

    // Use this for initialization
    protected virtual void Start()
    {
        m_camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
    }

    // Update is called once per frame
    protected virtual void Update()
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
                if (!turnedOn)
                {
                    turnedOn = true;
                    GetComponent<Renderer>().material = debugtmp_newMaterial;
                    m_camCtrl.cameraMoveTo(cameraPos.position, cameraPos.rotation);
                    TriggerEvent(this, "Activate", 1.0f);
                    TriggerEvent(this, "cameraRet", 3.0f);
                    TriggerEvent(this, "isOver", 4.0f);
                }
                break;

            case "Activate":
                TriggerEvent(targetObject, message);
                break;

            case "cameraRet":
                m_camCtrl.cameraMoveBack();
                break;

                //todo: camera movement
        }
    }
}
