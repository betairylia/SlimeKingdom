using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingLight_WG : eObject
{
    public eObject nextObject = null, finalObject = null;
    public float unlightTime = 6.0f;
    public float passTime = 0.3f;

    public Material onMat = null, offMat = null;

    bool lighted = false;

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

        if(obj.GetState("player") > 0)
        {
            TriggerEvent(this, "light");
        }
    }

    public override void OnSignalRecieved(eObject source, string signal)
    {
        if(signal.Equals("light") && lighted == false)
        {
            lighted = true;
            gameObject.GetComponent<Renderer>().material = onMat;
            TriggerEvent(this, "unlight", unlightTime);
            TriggerEvent(nextObject, "light", passTime);
            RegisterEvent(finalObject, "activate");
        }
        if(signal.Equals("unlight") && lighted == true)
        {
            lighted = false;
            gameObject.GetComponent<Renderer>().material = offMat;
            RemoveEvent(finalObject, "lighted");
        }
    }
}
