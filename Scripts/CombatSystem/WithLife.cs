using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithLife : eObject
{
	public float maxHealth, health;
    public eObject deadMessageTarget;
    public string deadMessage = "dead";

    protected Vector3 hitbackSpeed;

    // Use this for initialization
    void Start () 
	{
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public override void OnSignalRecieved(eObject source, string[] signal)
	{
		base.OnSignalRecieved (source, signal);
		switch (signal [0]) 
		{
			case "damage":
				Vector3 damageDirc;
				damageDirc.x = float.Parse (signal [2]);
				damageDirc.y = float.Parse (signal [3]);
				damageDirc.z = float.Parse (signal [4]);
				BeDamaged (float.Parse (signal [1]), signal, damageDirc);
				break;

            case "heal":
                BeHealed (float.Parse(signal[1]), signal);
                break;
		}
	}

    public virtual void BeHealed(float healing, string[] originalSignal, eObject sourceDoHeal = null)
    {
        health += healing;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

	public virtual void BeDamaged(float damage, string[] originalSignal, Vector3 dirc, eObject sourceDoDamage = null)
	{
        if (damage < health)
        {
            health -= damage;
        }
        else
        {
            health = 0;
            OnDeath(damage, dirc, sourceDoDamage);
            TriggerEvent(deadMessageTarget, deadMessage, 0);
        }
        dirc = transform.position - dirc;
		dirc.y = 0;
		hitbackSpeed = dirc.normalized * 5f;
	}

    protected virtual void OnDeath(float damage, Vector3 dirc, eObject sourceDoDamage)
    {

    }
}
