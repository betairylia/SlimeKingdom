using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMiko_Jinjya : NPCBase
{
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

        switch(GameDataKeeper.GetSingleton().currentProcess)
        {
            case GameMainProcessState.AimFor_AutumnShrine:
                if(m_talkerSphere)
                {
                    m_talkerSphere.passageName = "AutumnShrine_RiverMiko_Welcome";
                    m_talkerSphere.passageNameAgain = "AutumnShrine_RiverMiko_Welcome";
                }
                break;
            case GameMainProcessState.AutumnShrine_FindItem:
                if (m_talkerSphere)
                {
                    m_talkerSphere.passageName = "AutumnShrine_RiverMiko_Calmdown";
                    m_talkerSphere.passageNameAgain = "AutumnShrine_RiverMiko_Calmdown";
                }
                break;
            case GameMainProcessState.AimFor_SummerGreatTree:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
