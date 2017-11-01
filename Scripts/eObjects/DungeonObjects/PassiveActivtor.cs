using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveActivtor : eObject
{
    public string targetState, signal = "Activate";
    public eObject targetObj;

    bool activated = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        if (!activated)
        {
            if (obj.GetState(targetState) > 0)
            {
                TriggerEvent(targetObj, signal);
                activated = true;
            }
        }
    }
}
