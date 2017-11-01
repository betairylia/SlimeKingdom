using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : eObject
{
    public Vector3 windDirc { private set; get; }
    public Transform effect;
    public GameObject tmp;
    public float windSpeed;

	// Use this for initialization
	void Start ()
    {
        SetState("IgnoreProjectile");
        windDirc = windSpeed * (transform.rotation * Vector3.forward);

        effect.localScale = transform.localScale;

        Destroy(tmp);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectStay(eObject obj)
    {
        base.OnObjectStay(obj);

        TriggerEvent(obj, "inWind");
    }

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectStay(obj);

        TriggerEvent(obj, "enterWind");
    }

    public override void OnObjectExit(eObject obj)
    {
        base.OnObjectExit(obj);

        TriggerEvent(obj, "exitWind");
    }
}
