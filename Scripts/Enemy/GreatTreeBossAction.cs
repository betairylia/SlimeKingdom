using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossActionState
{
    NotStart,           //未激活（战斗未开始）
    Idle,               //空闲状态，可以进行移动
    Channeling,         //引导法术攻击 / 释放持续性法术（正在读条，无法移动）
    Casting,            //释放法术（如大跳）
    SlowedChanneling,   //引导法术中，缓慢移动
    Slowed,             //被减速
    Stunned,            //被眩晕
    Freezed,            //被冻结（仅无法移动）
    WaitForNewPlatform, //等待玩家前往新的平台
};

public class GreatTreeBossAction : Enemy
{
    [Space(10), Header("Targets & Consts")]
    public Transform m_target;
    public float m_moveSpeed, m_stopDistance;

    [Space(10), Header("AttackEffects")]
    public GameObject m_fxCharge;

    [Space(10), Header("AttackObjects")]
    public GameObject m_objBookshelf;

    [Space(10), Header("SpellParameters")]
    public float m_baseAttackTimeInterval;
    public float m_baseAttackChannelTime, m_baseAttackCastTime;
    public float m_jumpingTime, m_jumpingHeight;
    public AnimationCurve m_jumpingCurveHor, m_jumpingCurveVer;

    BossActionState m_state;
    float m_baseAttackTimer;

    float m_groundHeight;
    float m_jumpingTimer;//is Jumping if jumpingTimer > 0f
    Vector3 m_jumpingTargetPos, m_jumpingStartPos;

    int m_playerPlatform = 0;

    UIController m_UIController;

	// Use this for initialization
	void Start ()
    {
        m_baseAttackTimer = m_baseAttackTimeInterval;

        StopChargeEffect();

        m_UIController = GameObject.Find("UIController").GetComponent<UIController>();

        m_groundHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update ()
    {
        if(health <= maxHealth * 0.5f)
        {
            //TODO: go to Phase 2.
        }

        if(m_state != BossActionState.NotStart)
        {
            //Update mana and health status
            m_UIController.ShowBossBar();
            m_UIController.SetBossHealth(health / maxHealth);
            m_UIController.SetBossMana(1.0f - (m_baseAttackTimer / m_baseAttackTimeInterval));

            if (m_state != BossActionState.WaitForNewPlatform)
            {
                //Movement
                if (m_state == BossActionState.Idle ||
                    m_state == BossActionState.Slowed ||
                    m_state == BossActionState.SlowedChanneling)
                {
                    //Boss stops in front of player.
                    if ((m_target.position - transform.position).magnitude >= m_stopDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, m_target.position,
                            (m_state == BossActionState.Idle ? m_moveSpeed : m_moveSpeed * 0.4f) * Time.deltaTime);
                    }

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        Quaternion.LookRotation(m_target.position - transform.position, Vector3.up),
                        (m_state == BossActionState.Idle ? 180.0f : 80.0f) * Time.deltaTime);
                }

                //base attack(Heather's collection)
                m_baseAttackTimer -= Time.deltaTime;
                if(this.GetState("Charge_GT") > 0)
                {
                    //cooldown gets reduced when boss with in the effect area.
                    m_baseAttackTimer -= 0.6f * Time.deltaTime;
                }

                if (m_baseAttackTimer < 0 &&
                    (m_state == BossActionState.Idle ||
                     m_state == BossActionState.Slowed ||
                     m_state == BossActionState.Freezed))
                {
                    m_baseAttackTimer = m_baseAttackTimeInterval;
                    TriggerEvent(this, "baseAtk");
                }

                //Big Jump
                if(m_jumpingTimer > 0 && m_state == BossActionState.Casting)
                {
                    m_jumpingTimer -= Time.deltaTime;

                    transform.position = Vector3.Lerp(m_jumpingStartPos, m_jumpingTargetPos, m_jumpingCurveHor.Evaluate((m_jumpingTime - m_jumpingTimer) / m_jumpingTime)) + 
                        Vector3.up * m_jumpingHeight * m_jumpingCurveVer.Evaluate((m_jumpingTime - m_jumpingTimer) / m_jumpingTime);
                }
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
            case "Activate":
                m_state = BossActionState.Idle;
                break;
            case "toCasting":
                m_state = BossActionState.Casting;
                StopChargeEffect();
                break;
            case "toIdle":
                m_state = BossActionState.Idle;
                StopChargeEffect();
                break;
            case "toSlowedChanneling":
                m_state = BossActionState.SlowedChanneling;
                PlayChargeEffect();
                break;
            case "toChanneling":
                m_state = BossActionState.Channeling;
                PlayChargeEffect();
                break;
            //Heather's Collection
            case "baseAtk":
                TriggerEvent(this, "toSlowedChanneling");
                Instantiate(m_objBookshelf, transform.position, Quaternion.identity);
                TriggerEvent(this, "toCasting", m_baseAttackChannelTime);
                TriggerEvent(this, "toIdle", m_baseAttackChannelTime + m_baseAttackCastTime);
                break;
            case "playerExitPlatform":
                if(int.Parse(signal[1]) == m_playerPlatform)
                {
                    //m_playerPlatform = 0;

                    //Cancel all actions.
                    StopAllCoroutines();

                    m_state = BossActionState.WaitForNewPlatform;
                }
                break;
            case "playerEnterPlatform":
                if(m_state == BossActionState.WaitForNewPlatform && m_playerPlatform != int.Parse(signal[1]))
                {
                    //transform.position = source.transform.position;
                    //m_state = BossActionState.Idle;

                    m_jumpingTargetPos = source.transform.position;
                    m_jumpingTargetPos.y = m_groundHeight;

                    m_jumpingTimer = m_jumpingTime;
                    m_jumpingStartPos = transform.position;

                    TriggerEvent(this, "toChanneling", 0.0f);
                    TriggerEvent(this, "toCasting", 1.5f);
                    TriggerEvent(this, "jumpFinish", 1.5f + m_jumpingTime);
                }
                else if(m_playerPlatform == int.Parse(signal[1]))
                {
                    TriggerEvent(this, "toIdle");
                }
                
                //TEST ONLY TEST ONLY TEST ONLY
                if (m_state == BossActionState.NotStart)
                {
                    TriggerEvent(this, "Activate");
                }
                //TEST ONLY TEST ONLY TEST ONLY

                m_playerPlatform = int.Parse(signal[1]);
                break;
            case "jumpFinish":
                TriggerEvent(this, "toIdle");
                m_jumpingTimer = -1.0f;

                transform.position = m_jumpingTargetPos;
                break;
        }
    }
}
