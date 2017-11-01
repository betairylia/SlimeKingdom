using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Argur_InLordRoom : NPCBase
{
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

        gameObject.SetActive(false);

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Tutorial_Finished)
        {
            gameObject.SetActive(true);
        }
        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Solved_SummerGreatTree)
        {
            gameObject.SetActive(true);
        }
    }
}
