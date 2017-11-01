using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseAttack_obj : eObject
{
    public AttackActionType type;
    public float radius, yOffset, degree, degreeSelf;
    public float objDamage = 25;

    bool moving = true;

    // Use this for initialization
    void Start ()
    {
        degreeSelf = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(moving)
        {
            degree -= (type == AttackActionType.Base_1 ? 400.0f : -400.0f) * Time.deltaTime;

            transform.localPosition =
                Quaternion.AngleAxis(degree, Vector3.up) *
                (radius * Vector3.forward) +
                yOffset * Vector3.up;

            degreeSelf += 1600.0f * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(degreeSelf, Vector3.up);
        }
    }

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        TriggerEvent(obj, "damageToEnemy " + objDamage +
            " " + Toolbox.ToRawString(transform.position));
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "destroy":
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                moving = false;
                break;
        }
    }
}
