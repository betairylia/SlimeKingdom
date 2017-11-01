using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGT_BaseAttackCoreObject : eObject
{
    public GameObject boomEffect;
    public bool isStatic = false;

	// Use this for initialization
	void Start ()
    {
        SetState("GT_baco");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        if (obj.GetState("player") > 0)
        {
            //Hit player
            TriggerEvent(obj, "damage 30 " + Toolbox.ToRawString(transform.position));
        }

        if(isStatic)
        {
            Destroy(this);
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "boom":
                if(isStatic == false)
                {
                    rangedAttack atk = Instantiate(boomEffect, transform.position, Quaternion.identity).GetComponent<rangedAttack>();
                    atk.SetAttack(30.0f, 15);
                    Destroy(transform.parent.gameObject);
                }
                break;
        }
    }
}
