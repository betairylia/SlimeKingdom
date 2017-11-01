using UnityEngine;
using System.Collections;

public class SpecialSpringBoom : AttackAction
{
    public SpringBombObject springBomb;

    public SpecialSpringBoom(SlimeAttacker atker) : base(atker)
    {
        type = AttackActionType.SpecialSpringBoom;
        timeLeft = 0.3f;
        timeCombo = 0.3f;
    }

    public override void Init()
    {
        base.Init();

        springBomb = GameObject.Find("SpringBomb(Clone)").GetComponent<SpringBombObject>();
        springBomb.TriggerEvent(springBomb, "Activate", 0);
    }
}
