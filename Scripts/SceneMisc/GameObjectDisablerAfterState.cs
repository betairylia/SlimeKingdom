using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDisablerAfterState : NPCBase
{
    public GameObject[] objectList;
    public GameMainProcessState targetState;

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

        if(GameDataKeeper.GetSingleton().currentProcess >= targetState)
        {
            foreach(GameObject obj in objectList)
            {
                if(obj)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
