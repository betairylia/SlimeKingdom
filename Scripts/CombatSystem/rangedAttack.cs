using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedAttack : eObject
{
    public float damage, range;

	// Use this for initialization
	void Start ()
    {
        SetState("IgnoreProjectile");
    }

    //Need Refactoring
    public void SetAttack(float _damage, float _range)
    {
        damage = _damage;
        range = _range;

        SphereCollider coll = gameObject.AddComponent<SphereCollider>();
        coll.radius = range;
        coll.isTrigger = true;
        Destroy(coll, 0.2f);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);
        
        TriggerEvent(obj, "damageToPlayer " + damage + " " + Toolbox.ToRawString(transform.position));
    }
}
