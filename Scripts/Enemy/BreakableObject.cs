using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : Enemy
{
    public GameObject boomEffect;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    protected override void OnDeath(float damage, Vector3 dirc, eObject sourceDoDamage)
    {
        base.OnDeath(damage, dirc, sourceDoDamage);

        Instantiate(boomEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
