using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSummer : AttackAction
{
    public GameObject summerWhip;
    float _sx, _sy;

    public SpecialSummer(SlimeAttacker atker, float screenX, float screenY) : base(atker)
    {
        type = AttackActionType.SpecialSummer;
        timeLeft = 0.6f;
        timeCombo = 0.6f;

        _sx = screenX;
        _sy = screenY;
    }

    public override void Init()
    {
        base.Init();

        Vector3 targetPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(_sx * Screen.width, _sy * Screen.height, 100000.0f));

        summerWhip = GameObject.Instantiate(
            m_parent.summerWhip,
            m_parent.transform.position + 0.5f * Vector3.up,
            Quaternion.LookRotation(targetPoint - (m_parent.transform.position + 0.5f * Vector3.up), Vector3.up),
            m_parent.transform);

        summerWhip.GetComponent<SpecialSummer_Object>().SetParentSlime(m_parent.GetComponent<Slime>());
    }
}
