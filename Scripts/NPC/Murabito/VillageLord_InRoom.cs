using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageLord_InRoom : NPCBase
{
    public GameObject dialogTrigger;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void CheckState()
    {
        base.CheckState();

        dialogTrigger.SetActive(false);

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Tutorial_Finished)
        {
            dialogTrigger.SetActive(true);
            dialogTrigger.GetComponent<PassiveActivtor>().targetObj = GameObject.Find("Dialog_VeryBeginning").GetComponent<DialogHandlerWithCameraMovement>();
        }

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.GoingBackToVillage_WithSpringSprite)
        {
            dialogTrigger.SetActive(true);
            dialogTrigger.GetComponent<PassiveActivtor>().targetObj = GameObject.Find("Dialog_AfterSpringSprite").GetComponent<DialogHandlerWithCameraMovement>();
        }

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Solved_SummerGreatTree)
        {
            dialogTrigger.SetActive(true);
            dialogTrigger.GetComponent<PassiveActivtor>().targetObj = GameObject.Find("Dialog_AfterSummerGreatTree").GetComponent<DialogHandlerWithCameraMovement>();
        }
    }
}
