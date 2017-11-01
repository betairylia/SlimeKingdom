using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelldownArea : eObject
{
    public bool hidden;

    // Use this for initialization
    void Start()
    {
        if(GetComponent<Renderer>() && hidden)
            GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        TriggerEvent(obj, "felldown");
    }
}
