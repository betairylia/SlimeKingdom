using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WithLife
{
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
		switch (signal[0])
		{
			case "damageToPlayer":
				Vector3 damageDirc;
				damageDirc.x = float.Parse(signal[2]);
				damageDirc.y = float.Parse(signal[3]);
				damageDirc.z = float.Parse(signal[4]);
				BeDamaged(float.Parse(signal[1]), signal, damageDirc);
				break;

            case "healToPlayer":
                BeHealed(float.Parse(signal[1]), signal);
                break;
		}
	}
}
