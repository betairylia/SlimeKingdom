using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WithLife
{
    [SerializeField]
    private GameObject goldItem, manaItem, healthItem;
    public float goldAmountMax, manaAmountMax, healthAmountMax;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

	public override void OnSignalRecieved(eObject source, string[] signal)
	{
		base.OnSignalRecieved(source, signal);
		switch (signal[0])
		{
			case "damageToEnemy":
				Vector3 damageDirc;
				damageDirc.x = float.Parse(signal[2]);
				damageDirc.y = float.Parse(signal[3]);
				damageDirc.z = float.Parse(signal[4]);
				BeDamaged(float.Parse(signal[1]), signal, damageDirc);
				break;
            case "healToEnemy":
                BeHealed(float.Parse(signal[1]), signal);
                break;
		}
	}

    protected override void OnDeath(float damage, Vector3 dirc, eObject sourceDoDamage)
    {
        base.OnDeath(damage, dirc, sourceDoDamage);

        float manaPercentage = GameDataKeeper.GetSingleton().currentMagic / GameDataKeeper.GetSingleton().maxMagic;
        float goldAmount = (0.5f + 0.5f * manaPercentage) * goldAmountMax * Random.Range(0.7f, 1.0f);
        float manaAmount = (1.0f - manaPercentage) * manaAmountMax * Random.Range(0.6f, 1.0f);
        float healthAmount = (1.0f - (GameDataKeeper.GetSingleton().currentHealth / GameDataKeeper.GetSingleton().maxHealth)) * healthAmountMax * Random.Range(0.5f, 1.0f);
        
        for(int i = 0; i < goldAmount; i++)
        {
            Vector2 dircHor = Random.insideUnitCircle;
            Vector3 dircObj = new Vector3(dircHor.x, Random.Range(0.8f, 1.2f), dircHor.y).normalized;
            GameObject obj = Instantiate(goldItem, transform.position + dircObj * 0.5f, Quaternion.identity);
            obj.GetComponent<Rigidbody>().AddForce(dircObj * 200.0f);
        }

        for (int i = 0; i < manaAmount; i++)
        {
            Vector2 dircHor = Random.insideUnitCircle;
            Vector3 dircObj = new Vector3(dircHor.x, Random.Range(0.8f, 1.2f), dircHor.y).normalized;
            GameObject obj = Instantiate(manaItem, transform.position + dircObj * 0.5f, Quaternion.identity);
            obj.GetComponent<Rigidbody>().AddForce(dircObj * 200.0f);
        }

        for (int i = 0; i < healthAmount; i++)
        {
            Vector2 dircHor = Random.insideUnitCircle;
            Vector3 dircObj = new Vector3(dircHor.x, Random.Range(0.8f, 1.2f), dircHor.y).normalized;
            GameObject obj = Instantiate(healthItem, transform.position + dircObj * 0.5f, Quaternion.identity);
            obj.GetComponent<Rigidbody>().AddForce(dircObj * 200.0f);
        }
    }
}
