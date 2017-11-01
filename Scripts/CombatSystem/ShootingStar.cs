using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : projectile
{
    public Vector3 dirc;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up * 240.0f * Time.deltaTime, Space.Self);
        transform.Translate(dirc * 12.0f * Time.deltaTime, Space.World);
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        TriggerEvent(obj, "damageToEnemy " + 15.0f +
            " " + Toolbox.ToRawString(transform.position));

        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);
    }
}
