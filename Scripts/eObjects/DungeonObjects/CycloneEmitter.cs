using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycloneEmitter : eObject
{
    public GameObject cycloneObj;
    public float cycloneLenth, cycloneSpeed, cycloneGap;

    public bool activateFirst;
    
    bool m_activated;

	// Use this for initialization
	void Start ()
    {
        if(activateFirst)
        {
            m_activated = true;
            TriggerEvent(this, "cyclone");
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(155f / 255f, 240f / 255f, 208f / 255f, 1.0f);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawSphere(Vector3.zero, 0.5f);
        Gizmos.DrawCube(Vector3.forward * cycloneLenth * 0.5f, new Vector3(0.5f, 0.5f, cycloneLenth));

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.white;
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                m_activated = true;
                TriggerEvent(this, "cyclone");
                break;

            case "Disable":
                StopAllCoroutines();
                m_activated = false;
                break;

            case "cyclone":
                Cyclone cyc = Instantiate(cycloneObj, transform.position, Quaternion.identity).GetComponent<Cyclone>();
                cyc.velocity = transform.rotation * Vector3.forward * cycloneSpeed;
                TriggerEvent(cyc, "cyclone " + cycloneLenth / cycloneSpeed, 0.1f);
                Destroy(cyc.gameObject, cycloneLenth / cycloneSpeed + 0.5f);

                if(m_activated == true)
                {
                    TriggerEvent(this, "cyclone", cycloneGap);
                }
                break;
        }
    }
}
