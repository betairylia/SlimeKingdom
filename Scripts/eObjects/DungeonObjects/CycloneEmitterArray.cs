using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycloneEmitterArray : eObject
{
    public CycloneEmitter[] emitters;
    public int maxDistance;
    int roadId;

    public float cycloneGap;

	// Use this for initialization
	void Start ()
    {
        roadId = -1;
        TriggerEvent(this, "next");

        foreach(var c in emitters)
        {
            TriggerEvent(c, "Disable", 0.1f);
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
            case "next":
                if(roadId == -1)
                {
                    roadId = Random.Range(0, emitters.Length);
                }
                else
                {
                    int roadOffset = Random.Range(1, maxDistance + 1) * (Random.Range(-10.0f, 10.0f) > 0 ? (+1) : (-1));
                    roadId = roadId + roadOffset;
                    roadId = Mathf.Clamp(roadId, 0, emitters.Length - 1);
                }

                Debug.Log(roadId);

                for(int i = 0; i < emitters.Length; i++)
                {
                    if(i != roadId)
                    {
                        TriggerEvent(emitters[i], "cyclone");
                    }
                }

                TriggerEvent(this, "next", cycloneGap);
                break;
        }
    }
}
