using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGT_BaseAttackObject : eObject
{
    public GameObject m_fxCharge, m_coreObject, m_effectArea;
    Transform m_parent;

    public float m_timeCharge, m_timeCast, m_homingSpeed;

    bool m_isCasting = false, m_isStatic = false;
    Vector3 m_targetPos;
    Quaternion m_targetRotation;
    float m_rotateSpeed, m_targetDistance;
    public float m_areaExpandSpeed = 1.8f, m_areaExpandMax = 16.0f;

    // Use this for initialization
    void Start ()
    {
        SetState("BossGT_baseAtk");

        m_parent = GameObject.FindWithTag("Boss").transform;
        m_coreObject.transform.localScale = Vector3.zero;

        TriggerEvent(this, "cast", m_timeCharge);
        TriggerEvent(this, "static", m_timeCharge + m_timeCast * 0.5f);

        m_targetPos = m_parent.gameObject.GetComponent<GreatTreeBossAction>().m_target.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!m_isStatic)
        {
            if (!m_isCasting)
            {
                m_coreObject.transform.localScale = Vector3.MoveTowards(m_coreObject.transform.localScale, 2.0f * Vector3.one, 2.0f / (m_timeCharge * 0.6f) * Time.deltaTime);
                m_targetPos = Vector3.MoveTowards(m_targetPos, m_parent.gameObject.GetComponent<GreatTreeBossAction>().m_target.position, m_homingSpeed * Time.deltaTime);

                transform.position = m_parent.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, m_parent.rotation * (4.0f * Vector3.up + new Vector3(-1, 0, -1)));
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_targetRotation, m_rotateSpeed * 1.0f / (m_timeCast * 0.5f) * Time.deltaTime);
                m_coreObject.transform.localPosition = Vector3.MoveTowards(m_coreObject.transform.localPosition, m_targetDistance * Vector3.up, 8.0f * Time.deltaTime);
            }
        }
        else
        {
            if(m_effectArea.transform.localScale.x < m_areaExpandMax)
            {
                m_effectArea.transform.localScale = m_effectArea.transform.localScale + m_areaExpandSpeed * Time.deltaTime * (Vector3.forward + Vector3.right);
            }
        }
    }

    void PlayChargeEffect()
    {
        foreach (ParticleSystem ps in m_fxCharge.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
    }

    void StopChargeEffect()
    {
        foreach (ParticleSystem ps in m_fxCharge.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "cast":
                m_isCasting = true;

                StopChargeEffect();

                m_targetRotation = Quaternion.FromToRotation(
                    Vector3.up,
                    m_targetPos - transform.position);
                m_targetDistance = (m_targetPos - transform.position).magnitude;
                m_rotateSpeed = Quaternion.Angle(transform.rotation, m_targetRotation);

                Debug.Log(m_targetDistance);
                break;
            case "static":
                m_isStatic = true;
                m_coreObject.GetComponent<BossGT_BaseAttackCoreObject>().isStatic = true;
                m_effectArea = Instantiate(m_effectArea, new Vector3(m_coreObject.transform.position.x, 0.0f, m_coreObject.transform.position.z), Quaternion.identity);
                break;
        }
    }
}
