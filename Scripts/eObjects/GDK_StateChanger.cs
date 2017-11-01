using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDK_StateChanger : eObject
{
    public GameMainProcessState targetState;

	// Use this for initialization
	void Start ()
    {
		
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
                GameDataKeeper.GetSingleton().SetMainProcess(targetState);
                break;
        }
    }
}
