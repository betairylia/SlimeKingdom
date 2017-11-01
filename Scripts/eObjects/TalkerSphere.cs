using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkerSphere : eObject
{
    [HideInInspector]
    public DialogShower dShower;
    public string passageName, passageNameAgain;
    public eObject signalReciever;

    eObject dialogParent;
    bool dialogHappened;

    // Use this for initialization
    void Start ()
    {
        if(dShower == null)
        {
            dShower = GameObject.Find("DialogShower").GetComponent<DialogShower>();
        }
        dialogHappened = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        if(obj.GetState("player") > 0)
        {
            TriggerEvent(obj, "enterTalker");
        }
    }

    public override void OnObjectExit(eObject obj)
    {
        base.OnObjectExit(obj);

        if (obj.GetState("player") > 0)
        {
            TriggerEvent(obj, "exitTalker");
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "beginDialog":
                dialogParent = source;
                if(!dialogHappened)
                {
                    TriggerEvent(dShower, "dialog " + passageName);
                }
                else
                {
                    TriggerEvent(dShower, "dialog " + passageNameAgain);
                }
                break;
            case "dialogEnd":
                TriggerEvent(dialogParent, "dialogEnd");
                dialogParent = null;
                dialogHappened = true;
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
                TriggerEvent(signalReciever, str);
                break;
        }
    }
}
