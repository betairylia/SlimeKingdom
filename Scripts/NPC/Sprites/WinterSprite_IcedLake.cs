using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterSprite_IcedLake : NPCBase
{
    public GameObject dialogTrigger;
    public eObject targetEnemysToGo, talkerSphereHandler;

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

        dialogTrigger.SetActive(false);
        talkerSphereHandler.gameObject.SetActive(false);

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.AimFor_WinterRoad)
        {
            dialogTrigger.SetActive(true);
            dialogTrigger.GetComponent<PassiveActivtor>().targetObj = GameObject.Find("Dialog_WinterSpriteFirst").GetComponent<DialogHandlerWithCameraMovement>();
        }

        if(GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Battle_WinterSprite)
        {
            TriggerEvent(targetEnemysToGo, "Activate");
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Solved_WinterSprite)
        {
            talkerSphereHandler.gameObject.SetActive(true);
        }
    }
}
