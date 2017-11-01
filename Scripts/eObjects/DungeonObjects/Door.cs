using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : eObject
{
    bool opening = false;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (opening)
        {
            transform.Translate(Vector3.down * 5.0f * Time.deltaTime, Space.World);
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "Activate":
                opening = true;
                TriggerEvent(this, "destroy", 3.0f);
                break;
            case "destroy":
                Destroy(gameObject);
                break;
        }
    }
}
