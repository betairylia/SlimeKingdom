using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StrightBulletObj : projectile
{
    public float flySpeed, damage;
    public GameObject explodeFx;

    public Transform target;
    public float countDown = 0.15f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(flySpeed * Time.deltaTime * Vector3.forward);

        if(countDown > 0 && target != null)
        {
            countDown -= Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position + 0.3f * Vector3.up), 360.0f * Time.deltaTime);
        }
    }

    public override void OnTriggerEnter(Collider obj)
    {
        base.OnTriggerEnter(obj);

        if(obj.GetComponent<eObject>() == null)
        {
            Destroy(Instantiate(explodeFx, transform.position, Quaternion.identity), 0.5f);
            Destroy(gameObject);
        }
    }

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        Destroy(Instantiate(explodeFx, transform.position, Quaternion.identity), 0.5f);
        TriggerEvent(obj, "damage " + damage + " " + Toolbox.ToRawString(transform.position));
        Destroy(gameObject, 0.02f);
    }
}
