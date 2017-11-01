using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandlerWithCameraMovement : eObject
{
    public string passageName;
    eObject m_activator;

    [HideInInspector]
    public Slime m_playerSlime;
    [HideInInspector]
    public eObject dShower;
    [HideInInspector]
    public Transform cameraPos;
    [HideInInspector]
    public CameraController camCtrl;

    public Transform[] transformList;

    public GameMainProcessState state_changeTo = GameMainProcessState.None;

	// Use this for initialization
	void Start ()
    {
        m_playerSlime = GameObject.FindWithTag("Player").GetComponent<Slime>();
        camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
        dShower = GameObject.Find("DialogShower").GetComponent<DialogShower>();
        cameraPos = transform.Find("CameraHelper").transform;
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
                if(source != this)
                    m_activator = source;
                m_playerSlime.FreezePlayer();
                TriggerEvent(dShower, "dialog " + passageName);
                break;
            case "cameraMove":
                camCtrl.cameraMoveTo(cameraPos.position, cameraPos.rotation);
                break;
            case "cameraRet":
                camCtrl.cameraMoveBack();
                break;
            case "dialogEnd":
                camCtrl.cameraMoveBack();
                m_playerSlime.UnfreezePlayer();
                TriggerEvent(m_activator, "dialogEnd");
                break;
            case "translate":
                if(transformList != null)
                {
                    int index = int.Parse(signal[1]);
                    Vector3 pos = Toolbox.ToVec3(signal[2], signal[3], signal[4]);
                    Quaternion q = Quaternion.Euler(Toolbox.ToVec3(signal[5], signal[6], signal[7]));

                    if(index < transformList.Length)
                    {
                        transformList[index].position = pos;
                        transformList[index].rotation = q;
                    }
                }
                break;
            case "changeState":
                if(state_changeTo != GameMainProcessState.None)
                {
                    GameDataKeeper.GetSingleton().SetMainProcess(state_changeTo);
                }
                break;
            default:
                string str = "";
                for (int i = 0; i < signal.Length; i++)
                {
                    if (i < signal.Length - 1)
                    {
                        str = str + signal[i] + " ";
                    }
                    else
                    {
                        str = str + signal[i];
                    }
                }
                TriggerEvent(m_activator, str);
                break;
        }
    }
}
