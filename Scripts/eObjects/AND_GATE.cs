using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AND_GATE : eObject
{
    public Material powerOn = null;
    public int numActivate = 2;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(GetState("activate") >= numActivate)
        {
            gameObject.GetComponent<Renderer>().material = powerOn;
        }
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        //nothing to do.
    }
}
