using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutumnSprite_Shrine : NPCBase
{
    public ParticleSystem ps;
    public talkerSphere_handler m_talkerSphereHandler;
    public eObject playerSlime;

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

        if(GameDataKeeper.GetSingleton().currentProcess < GameMainProcessState.AutumnShrine_GoMeetAutumn)
        {
            ps.Stop();
            ps.Clear();
        }
        else
        {
            ps.Play();
        }

        switch(GameDataKeeper.GetSingleton().currentProcess)
        {
            case GameMainProcessState.AutumnShrine_GoMeetAutumn:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_AutumnSprite";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.AimFor_SummerGreatTree_After;
                break;
            case GameMainProcessState.AimFor_SummerGreatTree_After:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_AutumnSprite_After";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.None;
                break;
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "pickUp_Power":
                TriggerEvent(playerSlime, "picked AutumnSoul");
                break;
            case "pickUp_Key":
                TriggerEvent(playerSlime, "picked ActivateCrystal");
                break;
        }
    }
}
