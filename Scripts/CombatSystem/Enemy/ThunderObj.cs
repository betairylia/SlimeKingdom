using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderObj : eObject
{
    public float damageTotal;
    public ParticleSystem m_thunderCore, m_thunderFx, m_attackArea, m_attackCharge;

	// Use this for initialization
	void Start ()
    {
        m_attackCharge.Play();
        m_attackArea.Play();

        m_thunderCore.Stop();
        m_thunderFx.Stop();

        GetComponent<SphereCollider>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectStay(eObject obj)
    {
        base.OnObjectStay(obj);

        TriggerEvent(obj, "damageToPlayer " + damageTotal * Time.fixedDeltaTime * 3.33f + " " + Toolbox.ToRawString(transform.position));
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                m_thunderCore.Play();
                TriggerEvent(this, "attackFx", 0.2f);
                TriggerEvent(this, "attackFinish", 0.5f);
                TriggerEvent(this, "attackFinishFx", 2.0f);
                break;

            case "attackFx":
                m_thunderFx.Play();
                m_attackArea.Stop();
                m_attackCharge.Stop();
                GetComponent<SphereCollider>().enabled = true;
                break;

            case "attackFinish":
                GetComponent<SphereCollider>().enabled = false;
                break;

            case "attackFinishFx":
                Destroy(this.gameObject);
                break;
        }
    }
}
