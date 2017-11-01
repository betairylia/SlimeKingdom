using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test_levelTransporter : eObject
{
    public string targetSceneName = "MassiveDetailDebugScene";

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        if (obj.GetState("player") > 0)
        {
            TransportTo(targetSceneName);
        }
    }

    private void TransportTo(string sceneName)
    {
        GameDataKeeper.GetSingleton().prevSceneName = SceneManager.GetActiveScene().name;
        GameDataKeeper.GetSingleton().targetSceneName = sceneName;

        Debug.Log(GameDataKeeper.GetSingleton().prevSceneName + "->" + GameDataKeeper.GetSingleton().targetSceneName);

        SceneManager.LoadScene("LoadingScene");
    }
}
