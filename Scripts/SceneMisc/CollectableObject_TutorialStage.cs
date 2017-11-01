using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject_TutorialStage : eObject
{
    public string objectName;
    public DialogHandlerWithCameraMovement dialogHandler;
    eObject m_source;

    // Use this for initialization
    void Start()
    {
        SetState("CollectableObject");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "pickUp":
                m_source = source;
                TriggerEvent(dialogHandler, "Activate");
                break;
            case "pickUp_Real":
                TriggerEvent(m_source, "picked " + objectName);
                GetComponent<Renderer>().enabled = false;
                Destroy(gameObject, 0.1f);
                break;
            case "waitForCloseInventory":
                GameObject.Find("UIController").GetComponent<UIController>().Tutorial_WaitForCloseInventory();
                break;

        }
    }
}
