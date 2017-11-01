using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarter : NPCBase
{
    public DialogHandlerWithCameraMovement dialogHandler;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void CheckState()
    {
        base.CheckState();

        if(GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.NewStart_Tutorial)
        {
            dialogHandler.TriggerEvent(dialogHandler, "Activate");

            GameObject.Find("UIController").GetComponent<UIController>().BlackScreenOn();
        }
    }
}
