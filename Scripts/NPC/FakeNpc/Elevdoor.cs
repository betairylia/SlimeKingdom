using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevdoor : NPCBase
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

        if(GameDataKeeper.GetSingleton().itemCount[(int)ItemClass.ActivateCrystal] >= 1)
        {
            m_talkerSphere.passageName = "SummerForest2_Elevator_HasKey";
            m_talkerSphere.passageNameAgain = "SummerForest2_Elevator_HasKey";
        }
        else
        {
            m_talkerSphere.passageName = "SummerForest2_Elevator_NoKey";
            m_talkerSphere.passageNameAgain = "SummerForest2_Elevator_NoKey";
        }
    }
}
