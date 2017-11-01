using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaimikoSama_Jinjya : NPCBase
{
    talkerSphere_handler m_talkerSphereHandler;

    protected override void Start()
    {
        m_talkerSphereHandler = gameObject.GetComponentInChildren<talkerSphere_handler>();
        base.Start();
    }

    public override void CheckState()
    {
        base.CheckState();

        switch(GameDataKeeper.GetSingleton().currentProcess)
        {
            case GameMainProcessState.AimFor_AutumnShrine:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_DaimikoSama_FirstImpression";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.AutumnShrine_FindItem;
                break;
            case GameMainProcessState.AutumnShrine_FindItem:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_DaimikoSama_Calmdown";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.None;
                break;
            case GameMainProcessState.AutumnShrine_ItemFoundALL:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_DaimikoSama_ItemFoundALL";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.AutumnShrine_GoMeetAutumn;
                break;
            case GameMainProcessState.AutumnShrine_GoMeetAutumn:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_GO!FOR!AUTUMN!";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.None;
                break;
            case GameMainProcessState.AimFor_SummerGreatTree_After:
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).passageName = "AutumnShrine_DaimikoSama_GoodBye";
                ((DialogHandlerWithCameraMovement)(m_talkerSphereHandler.handler)).state_changeTo = GameMainProcessState.None;
                break;
        }
    }
}
