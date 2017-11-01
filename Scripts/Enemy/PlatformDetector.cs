using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetector : eObject
{
    public eObject m_bossObject;
    public int m_platformNumber;

	// Use this for initialization
	void Start ()
    {
        SetState("IgnoreProjectile");
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
            TriggerEvent(m_bossObject, "playerEnterPlatform " + m_platformNumber);
        }
    }

    public override void OnObjectExit(eObject obj)
    {
        base.OnObjectExit(obj);

        if (obj.GetState("player") > 0)
        {
            TriggerEvent(m_bossObject, "playerExitPlatform " + m_platformNumber);
        }
    }
}
