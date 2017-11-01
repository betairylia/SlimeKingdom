using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : eObject
{
    public string objectName, uniqueName;
    public bool repeatable;

	// Use this for initialization
	void Start ()
    {
        if(repeatable == false && GameDataKeeper.GetSingleton().GetObjectState(uniqueName) == true)
        {
            //Destroy this cuz it has been already picked up.
            Destroy(gameObject, 0.05f);
        }
        SetState("CollectableObject");
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "pickUp":
                if(repeatable == false)
                {
                    GameDataKeeper.GetSingleton().SetObjectState(uniqueName, true);
                }
                TriggerEvent(source, "picked " + objectName);
                GetComponent<Renderer>().enabled = false;
                Destroy(gameObject, 0.1f);
                break;
        }
    }
}
