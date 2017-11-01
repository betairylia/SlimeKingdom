using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : eObject
{
    public GameObject[] tabs;

	// Use this for initialization
	void Start ()
    {
		foreach(GameObject obj in tabs)
        {
            obj.SetActive(false);
        }
        ShowPanel(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void CloseAllPanel()
    {
        foreach (GameObject obj in tabs)
        {
            obj.SetActive(false);
        }
    }

    public void ShowPanel(int id)
    {
        CloseAllPanel();
        tabs[id].SetActive(true);
    }
}
