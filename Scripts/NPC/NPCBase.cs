using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : eObject
{
    protected GameDataKeeper m_dataKeeper;
    protected TalkerSphere m_talkerSphere;

    // Use this for initialization
    protected virtual void Start ()
    {
        m_dataKeeper = GameDataKeeper.GetSingleton();
        m_talkerSphere = gameObject.GetComponentInChildren<TalkerSphere>();
        CheckState();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public virtual void CheckState()
    {

    }
}
