using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGT_BaseAttackAreaEffect : eObject
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		    
	}

    public override void OnObjectStay(eObject obj)
    {
        base.OnObjectEnter(obj);

        if (obj.GetState("player") > 0)
        {
            //Damage player
            TriggerEvent(obj, "DmgSlow_GT");
        }
        if (obj.GetState("boss") > 0)
        {
            //Charge boss
            if (obj.GetState("Charge_GT") == 0)
            {
                RegisterEvent(obj, "Charge_GT");
            }
        }
        if(obj.GetState("GT_baco") > 0)
        {
            TriggerEvent(obj, "boom");
        }
    }
}
