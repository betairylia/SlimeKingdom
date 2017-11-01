using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackActionType
{
    Base_1,
    Base_2,
    SpecialOrigin,
    SpecialSpring,
    SpecialSpringBoom,
    SpecialSummer,
    SpecialAutumn,
    SpecialWinter
};

public class AttackAction
{
    public static float[] manaCost =
    {
        0,
        0,
        10,
        30,
        0,
        15,
        20,
        15
    };

    public AttackAction(SlimeAttacker atker)
    {
        m_parent = atker;
        m_transform = m_parent.transform;
    }

    public virtual void Init()
    {

    }

    public virtual void Update(float dt)
    {
        timeLeft -= dt;
        timeCombo -= dt;
    }

    public bool isOver()
    {
        return (timeLeft <= 0);
    }

    public bool isEndCombo()
    {
        return (timeCombo <= 0);
    }

    protected SlimeAttacker m_parent;
    protected Transform m_transform;

    public AttackActionType type;
    public float timeLeft, timeCombo;
};
