using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDoor : eObject
{
    Transform pivot, parent;
    public GameObject crystal;
    bool opening = false;

	// Use this for initialization
	void Start ()
    {
        pivot = transform.parent.Find("Pivot");
        parent = transform.parent;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(opening)
        {
            parent.Translate(Vector3.down * 5.0f * Time.deltaTime, Space.World);
        }
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        if(obj.GetState("player") > 0)
        {
            Debug.Log("Player!");
            TriggerEvent(obj, "useCrystal");
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                GameObject fakeCrystal = Instantiate(crystal, pivot.position, Quaternion.identity);
                Destroy(fakeCrystal.GetComponent<CollectableObject>());
                Destroy(fakeCrystal, 1.0f);
                TriggerEvent(this, "open", 1.0f);
                break;
            case "open":
                opening = true;
                TriggerEvent(this, "destroy", 3.0f);
                break;
            case "destroy":
                Destroy(parent.gameObject);
                break;
        }
    }
}
