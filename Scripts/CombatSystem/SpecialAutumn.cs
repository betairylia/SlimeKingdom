using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAutumn : AttackAction
{
	public SpecialAutumn(SlimeAttacker atker) : base(atker)
    {
        type = AttackActionType.SpecialAutumn;
        timeLeft = 1.0f;
        timeCombo = 1.0f;
    }

    public override void Init()
    {
        base.Init();

        m_parent.GetComponent<Slime>().AutumnOpen();
    }
}
