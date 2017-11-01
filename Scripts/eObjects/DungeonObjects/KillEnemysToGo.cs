using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemysToGo : eObject
{
    public int targetEnemyCount;
    int deadEnemyCount;
    public eObject repeatableDoor, externalObject;
    public string activateMessage = "Activate", disableMessage = "Disable";
    public string enemyDeadMessage = "enemyDead";

    bool activated = false, finished = false;

    public GameObject[] enemys;

    Transform m_camHelper;
    CameraController m_camCtrl;

	// Use this for initialization
	void Start ()
    {
        m_camHelper = transform.Find("CameraHelper");
        m_camCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();

        foreach(GameObject obj in enemys)
        {
            obj.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(deadEnemyCount >= targetEnemyCount && activated && !finished)
        {
            TriggerEvent(this, "finish", 0);
        }
	}

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                if(!activated)
                {
                    activated = true;
                    if(m_camHelper)
                    {
                        m_camCtrl.cameraMoveTo(m_camHelper.position, m_camHelper.rotation);
                        TriggerEvent(repeatableDoor, disableMessage, 1.0f);
                        TriggerEvent(this, "cameraRet", 3.0f);
                        TriggerEvent(this, "spawnEnemy", 4.25f);
                    }
                    else
                    {
                        TriggerEvent(repeatableDoor, disableMessage);
                        TriggerEvent(this, "spawnEnemy");
                    }
                    deadEnemyCount = 0;
                }
                break;
            case "finish":
                if(!finished)
                {
                    finished = true;
                    if(m_camHelper)
                    {
                        m_camCtrl.cameraMoveTo(m_camHelper.position, m_camHelper.rotation);
                        TriggerEvent(repeatableDoor, activateMessage, 1.0f);
                        TriggerEvent(this, "cameraRet", 3.0f);
                        TriggerEvent(externalObject, activateMessage, 4.25f);
                    }
                    else
                    {
                        TriggerEvent(repeatableDoor, activateMessage);
                        TriggerEvent(externalObject, activateMessage);
                    }
                }
                break;
            case "spawnEnemy":
                foreach(GameObject obj in enemys)
                {
                    obj.SetActive(true);
                }
                break;
            case "cameraRet":
                m_camCtrl.cameraMoveBack();
                break;
        }

        if(signal[0] == enemyDeadMessage)
        {
            deadEnemyCount++;
        }
    }
}
