using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miko_River : NPCBase
{
    public GameObject dialogActivator;

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

        if(!(m_dataKeeper.currentProcess == GameMainProcessState.AimFor_SummerGreatTree ||
             m_dataKeeper.currentProcess == GameMainProcessState.SummerForest1_FindBridge))
        {
            gameObject.SetActive(false);
        }

        if(m_dataKeeper.currentProcess != GameMainProcessState.AimFor_SummerGreatTree)
        {
            dialogActivator.SetActive(false);
        }
    }
}
