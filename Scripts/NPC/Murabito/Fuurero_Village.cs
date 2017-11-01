using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuurero_Village : NPCBase
{
    public GameObject dialogTrigger_VeryBeginning;

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

        dialogTrigger_VeryBeginning.SetActive(false);
        gameObject.SetActive(false);

        if (GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.Tutorial_Finished)
        {
            dialogTrigger_VeryBeginning.SetActive(true);
            gameObject.SetActive(true);

            transform.position = new Vector3(108.46f, 2.83f, 145.73f);
            transform.eulerAngles = new Vector3(0, -14.814f, 0);
        }
        else if(GameDataKeeper.GetSingleton().currentProcess == GameMainProcessState.AimFor_SpringFlowerSea)
        {
            gameObject.SetActive(true);

            transform.position = new Vector3(135.46f, 2.83f, 164.83f);
            transform.eulerAngles = new Vector3(0, -200.587f, 0);
        }
    }
}
