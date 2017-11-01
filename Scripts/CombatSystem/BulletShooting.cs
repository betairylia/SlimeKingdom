using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooting : projectile
{
    public float orbDamage, bombDamage, range;
	public float speed, speedVertical, gravity, finalSpeedVertical;
    Vector3 speedVec;
	public GameObject bullet;
    public GameObject boomEffect;

    bool inWind = false;

	// Use this for initialization
	void Start ()
    {
        speedVec = new Vector3(0.0f, speedVertical, speed);
        speedVec = transform.rotation * speedVec;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(speedVec * Time.deltaTime, Space.World);

        if(!inWind)
        {
            speedVec.y -= gravity * Time.deltaTime;
            if (speedVec.y < finalSpeedVertical)
            {
                speedVec.y = finalSpeedVertical;
            }
        }
	}

    public void Boom()
    {
        GameObject rangedBomb = Instantiate(boomEffect, transform.position, transform.rotation);
        rangedBomb.GetComponent<rangedAttack>().SetAttack(bombDamage, range);

		gameObject.SetActive(false);
		Destroy(gameObject,0.1f);
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "enterWind":

                inWind = true;

                Vector3 windDirc = Toolbox.ToVec3(signal[1], signal[2], signal[3]);
                //speedVec.y = 0;
                //speedVec = Vector3.MoveTowards(speedVec, windDirc, 5.0f);
                speedVec = windDirc;
                break;
            case "exitWind":
                inWind = false;
                break;
        }
    }

    public override void OnTriggerEnter ( Collider collider )
	{
        base.OnTriggerEnter(collider);

        if(collider.gameObject.GetComponent<eObject>() == null)
        {
            //Debug.Log(collider.gameObject);
            Boom();
        }
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        //if(obj.GetState("player") == 0)
        //{
            TriggerEvent(obj, "damageToPlayer " + orbDamage +
                " " + Toolbox.ToRawString(transform.position));

            Boom();
        //}
    }
}
