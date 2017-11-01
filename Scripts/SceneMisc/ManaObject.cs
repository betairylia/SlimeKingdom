using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaObject : eObject
{
    public eObject m_target;
    bool activated = false;

    // Use this for initialization
    void Start()
    {
        SetState("autoCollect");
        TriggerEvent(this, "Activate", 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target && activated)
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            transform.position = Vector3.MoveTowards(transform.position, m_target.transform.position + Vector3.up, 10.0f * Time.deltaTime);

            if (Vector3.Distance(transform.position, m_target.transform.position + Vector3.up) <= 0.8f)
            {
                TriggerEvent(m_target, "picked mana");
                gameObject.SetActive(false);
                Destroy(gameObject, 0.1f);
            }
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "pickUp":
                m_target = source;
                break;
            case "Activate":
                activated = true;
                break;
        }
    }
}
