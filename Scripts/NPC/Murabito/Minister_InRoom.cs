using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minister_InRoom : NPCBase
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

        if (GameDataKeeper.GetSingleton().currentProcess != GameMainProcessState.Tutorial_Finished)
        {
        }
    }
}
