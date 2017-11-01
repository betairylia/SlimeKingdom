using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateableSwitches : eObject
{
    public bool activated;
    public string saveableName;

	// Use this for initialization
	protected virtual void Start ()
    {
		if(saveableName != null)
        {
            activated = GameDataKeeper.GetSingleton().GetObjectState(saveableName);
            if(activated)
            {
                TriggerEvent(this, "Activate");
            }
            else
            {
                TriggerEvent(this, "Disable");
            }
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
                OnActivated();
                break;
            case "Disable":
                OnDisabled();
                break;
        }
    }

    public virtual void OnActivated()
    {
        if (saveableName != null)
            GameDataKeeper.GetSingleton().SetObjectState(saveableName, true);
    }

    public virtual void OnDisabled()
    {
        if (saveableName != null)
            GameDataKeeper.GetSingleton().SetObjectState(saveableName, false);
    }
}
