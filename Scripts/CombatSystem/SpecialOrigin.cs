using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialOrigin : AttackAction
{
    public GameObject star;
    float _sx, _sy;

    public SpecialOrigin(SlimeAttacker atker, float screenX, float screenY) : base(atker)
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
            new Vector3(_sx * Screen.width, _sy * Screen.height, 10.0f));

        star = GameObject.Instantiate(
            m_parent.star,
            m_parent.transform.position + m_parent.transform.rotation * ( 0.5f * Vector3.up + 1.0f * Vector3.forward ),
            Quaternion.LookRotation(targetPoint - (m_parent.transform.position + m_parent.transform.rotation * (0.5f * Vector3.up + 1.0f * Vector3.forward)), Vector3.up));

        star.GetComponent<ShootingStar>().dirc = Quaternion.LookRotation(targetPoint - (m_parent.transform.position + 0.5f * Vector3.up), Vector3.up) * Vector3.forward;
    }
}
