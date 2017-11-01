using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InMapTeleporter : NPCBase
{
    public DialogHandlerWithCameraMovement[] handlers = new DialogHandlerWithCameraMovement[4];
    public Transform[] teleportPivots = new Transform[4];
    public talkerSphere_handler m_sphere;

    public Transform slimeTransform;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "teleport":
                int target = int.Parse(signal[1]);
                slimeTransform.position = teleportPivots[target].position;
                break;
        }
    }

    public override void CheckState()
    {
        base.CheckState();

        if(GameDataKeeper.GetSingleton().currentProcess < GameMainProcessState.SummerGreatTree_TP1)
        {
            m_sphere.handler = handlers[0];
        }
        else if(GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.SummerGreatTree_TP1)
        {
            m_sphere.handler = handlers[1];
        }
        else if(GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.SummerGreatTree_TP2)
        {
            m_sphere.handler = handlers[2];
        }
        else if(GameDataKeeper.GetSingleton().currentProcess >= GameMainProcessState.SummerGreatTree_TP3)
        {
            m_sphere.handler = handlers[3];
        }
    }
}
