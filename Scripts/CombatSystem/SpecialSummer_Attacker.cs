using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSummer_Attacker : eObject
{
    Slime m_slime;

    private void Start()
    {
        SetState("IgnoreSwitch");
    }

    bool canHook = true;

    public void SetParentSlime(Slime slime)
    {
        m_slime = slime;
    }

    public void EnableHook()
    {
        canHook = true;
    }

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        TriggerEvent(obj, "damageToEnemy 20 " + Toolbox.ToRawString(transform.parent.position));

        if(canHook && obj.GetState("hookPoint") > 0)
        {
            TriggerEvent(m_slime, "hook " + Toolbox.ToRawString(obj.transform.position));
        }
    }
}
