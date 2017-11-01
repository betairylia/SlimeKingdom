using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FloatingGun : eObject
{
    public GameObject attackObject;
    public float attackGap, attackRange, attackGapSmall, floatingSpeed, floatingRange;
    public int singleAttackCount;
    public Transform target;

    // Use this for initialization
    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        TriggerEvent(this, "attack", 0f, false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //(cos(0.8t), sin(1.7t))
        transform.localPosition = new Vector3(floatingRange * Mathf.Cos(0.8f * floatingSpeed * Time.time), floatingRange * Mathf.Sin(1.9f * floatingSpeed * Time.time));

        //Look to player
        transform.rotation = Quaternion.LookRotation(target.position + 0.3f * Vector3.up - transform.position);
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "attack":
                if(Vector3.Distance(target.position, transform.position) < attackRange)
                {
                    TriggerEvent(this, "attackOn " + singleAttackCount, 0.0f, false);
                }
                TriggerEvent(this, "attack", attackGap, false);
                break;

            case "attackOn":

                int index = int.Parse(signal[1]);
                if (index <= 0)
                    break;

                Instantiate(attackObject, transform.position + transform.rotation * new Vector3(0.0f, 0.3f, 0.75f), transform.rotation).
                    GetComponent<Enemy_StrightBulletObj>().target = target;
                TriggerEvent(this, "attackOn " + (index - 1), attackGapSmall, false);
                break;
        }
    }
}
