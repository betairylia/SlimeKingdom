using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class testEnemy : Enemy
{
    Vector3 playerPos;
    bool hasPos = false;

    public Transform lifeBar;
	public float speedLimDegree, speedLimValue, stopDist; 
	//public float maxHealth;
 //   float health;
 //   Vector3 hitbackSpeed;

	public GameObject testAttackObject;

    private CharacterController m_controller;

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        m_controller = gameObject.GetComponent<CharacterController>();
		TriggerEvent (this, "attack", 0, false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(hasPos)
        {
            Vector3 posDiff = playerPos - transform.position;
            posDiff.y = 0;

            
            Quaternion target = Quaternion.FromToRotation(
                Vector3.forward,
                posDiff);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speedLimDegree * Time.deltaTime);

            if (posDiff.magnitude >= stopDist)
            {
                m_controller.Move(speedLimValue * (transform.rotation * Vector3.forward) * Time.deltaTime);
            }

        }

        transform.Translate(hitbackSpeed * Time.deltaTime, Space.World);
        hitbackSpeed = Vector3.MoveTowards(hitbackSpeed, Vector3.zero, 10f * Time.deltaTime); //10: hitback reduce speed

        if(health <= 0)
        {
            Instantiate(gameObject, new Vector3(25, 2, -10), Quaternion.identity);
            Destroy(gameObject);
        }

        lifeBar.localScale = new Vector3((float)health / maxHealth * 2.0f, lifeBar.localScale.y, lifeBar.localScale.z);
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "playerPos":
                hasPos = true;
                playerPos.x = float.Parse(signal[1]);
                playerPos.y = float.Parse(signal[2]);
                playerPos.z = float.Parse(signal[3]);
                break;

            //case "damage":
            //    health -= float.Parse(signal[1]);
            //    Debug.Log(health);
            //    Vector3 diff;
            //    diff.x = float.Parse(signal[2]);
            //    diff.y = float.Parse(signal[3]);
            //    diff.z = float.Parse(signal[4]);

            //    diff = transform.position - diff;
            //    diff.y = 0;
            //    hitbackSpeed = diff.normalized * 5f; //5: hitback speed
            //    break;

			case "attack":
				TriggerEvent (this, "attack", 2f, false);
				if (hasPos) 
				{	
					Instantiate (testAttackObject, transform.position + transform.rotation * new Vector3 (0.0f, 0.5f, 0.75f), transform.rotation);
				}
				break;

			case "playerEnter":
				//TriggerEvent (this, "attack");
				hasPos = true;
				break;

			case "playerExit":
				hasPos = false;
				break;
		}
    }
}
