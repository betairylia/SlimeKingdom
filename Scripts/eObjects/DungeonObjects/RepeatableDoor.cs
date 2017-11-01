using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatableDoor : eObject
{
    bool opening = false, closing = false, opened = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (opening && !opened)
        {
            transform.Translate(Vector3.down * 5.0f * Time.deltaTime, Space.World);
        }
        if (closing && opened)
        {
            transform.Translate(Vector3.up * 5.0f * Time.deltaTime, Space.World);
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "Activate":
                if(!opening && !opened)
                {
                    opening = true;
                    TriggerEvent(this, "opened", 3.0f);
                }
                break;
            case "opened":
                opening = false;
                opened = true;
                break;
            case "Disable":
                if(!closing && opened)
                {
                    closing = true;
                    TriggerEvent(this, "closed", 3.0f);
                }
                break;
            case "closed":
                closing = false;
                opened = false;
                break;
        }
    }
}
