using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclone : eObject
{
    public Transform cyclonePivot;
    public Vector3 velocity;
    float m_lifeSpan = -1.0f, m_lifeTotal;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_lifeSpan > 0)
        {
            GetComponent<BoxCollider>().enabled = true;

            m_lifeSpan -= Time.deltaTime;

            float scaleSize = Mathf.Min(Mathf.Min((m_lifeTotal - m_lifeSpan) / 0.3f, 1.0f), Mathf.Min(m_lifeSpan / 0.3f, 1.0f));
            transform.localScale = Vector3.one * scaleSize;

            transform.Rotate(Vector3.up * 360.0f * Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime, Space.World);
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
            transform.localScale = Vector3.zero;
        }
    }

    public override void OnObjectStay(eObject obj)
    {
        base.OnObjectStay(obj);

        TriggerEvent(obj, "inCyclone");
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "cyclone":
                m_lifeSpan = float.Parse(signal[1]);
                m_lifeTotal = m_lifeSpan;
                break;
        }
    }
}
