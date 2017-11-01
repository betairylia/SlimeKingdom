using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseAttack_1 : AttackAction
{
    public GameObject attackObj;
    float radius, degree, degreeSelf, yOffset;

    public baseAttack_1(SlimeAttacker atker) : base(atker)
    {
        type = AttackActionType.Base_1;
        timeLeft = atker.timeBaseAttackTot;
        timeCombo = atker.timeBaseAttackLast;
        radius = atker.radius;
        yOffset = 0.569f;
    }

    public override void Update(float dt)
    {
        base.Update(dt);
    }

    public override void Init()
    {
        base.Init();

        degree = 60.0f;

        attackObj = GameObject.Instantiate(m_parent.attackObject, m_transform.position, m_transform.rotation, m_transform);
        attackObj.GetComponentInChildren<baseAttack_obj>().TriggerEvent(
            attackObj.GetComponentInChildren<baseAttack_obj>(), "destroy", m_parent.timeBaseAttack);
        GameObject.Destroy(attackObj, 1.0f);

        attackObj.GetComponentInChildren<baseAttack_obj>().radius = radius;
        attackObj.GetComponentInChildren<baseAttack_obj>().yOffset = yOffset;
        attackObj.GetComponentInChildren<baseAttack_obj>().type = type;
        attackObj.GetComponentInChildren<baseAttack_obj>().degree = degree;
    }
}
