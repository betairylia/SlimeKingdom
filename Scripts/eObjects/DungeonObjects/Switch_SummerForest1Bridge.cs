using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_SummerForest1Bridge : SwitchWithCamera
{
    public DialogHandlerWithCameraMovement dHandler;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "isOver":
                TriggerEvent(dHandler, "Activate");
                break;
        }
    }
}
