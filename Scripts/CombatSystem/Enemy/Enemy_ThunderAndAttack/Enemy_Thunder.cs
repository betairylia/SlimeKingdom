using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Thunder : Enemy
{
    Vector3 playerPos;
    bool hasPos = false, slowed = false;

    public GameObject m_thunderPrefab;

    public Transform lifeBar;
    public float speedLimDegree, speedLimValue, stopDist;
    //public float maxHealth;
    //   float health;
    //   Vector3 hitbackSpeed;

    public GameObject attackObject;
    public float attackGap, thunderGap, channelingTime, attackRange;
    public GameObject m_fxCharge;

    private CharacterController m_controller;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        m_controller = gameObject.GetComponent<CharacterController>();
        TriggerEvent(this, "attack", 0, false);
        TriggerEvent(this, "attackThunder", 0, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPos)
        {
            Vector3 posDiff = playerPos - transform.position;
            posDiff.y = 0;


            Quaternion target = Quaternion.FromToRotation(
                Vector3.forward,
                posDiff);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, speedLimDegree * Time.deltaTime * (slowed ? 0.4f : 1.0f));

            if (posDiff.magnitude >= stopDist)
            {
                m_controller.Move(speedLimValue * (transform.rotation * Vector3.forward) * Time.deltaTime * (slowed ? 0.3f : 1.0f));
            }
        }

        if (slowed)
        {
            PlayChargeEffect();
        }
        else
        {
            StopChargeEffect();
        }

        transform.Translate(hitbackSpeed * Time.deltaTime, Space.World);
        hitbackSpeed = Vector3.MoveTowards(hitbackSpeed, Vector3.zero, 10f * Time.deltaTime); //10: hitback reduce speed

        lifeBar.localScale = new Vector3((float)health / maxHealth * 2.0f, lifeBar.localScale.y, lifeBar.localScale.z);
    }

    protected override void OnDeath(float damage, Vector3 dirc, eObject sourceDoDamage)
    {
        base.OnDeath(damage, dirc, sourceDoDamage);

        Destroy(gameObject);
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

        switch (signal[0])
        {
            case "playerPos":
                hasPos = true;
                playerPos.x = float.Parse(signal[1]);
                playerPos.y = float.Parse(signal[2]);
                playerPos.z = float.Parse(signal[3]);
                break;

            //case "damage":
            //    health -= float.Parse(signal[1]);
            //    Debug.Log(health);
            //    Vector3 diff;
            //    diff.x = float.Parse(signal[2]);
            //    diff.y = float.Parse(signal[3]);
            //    diff.z = float.Parse(signal[4]);

            //    diff = transform.position - diff;
            //    diff.y = 0;
            //    hitbackSpeed = diff.normalized * 5f; //5: hitback speed
            //    break;

            case "attack":
                TriggerEvent(this, "attack", attackGap, false);

                if (hasPos && Vector3.Distance(transform.position, playerPos) <= attackRange)
                {
                    TriggerEvent(this, "attackGo", channelingTime, false);
                    slowed = true;
                }
                break;

            case "attackGo":
                Instantiate(attackObject, transform.position + transform.rotation * new Vector3(0.0f, 0.3f, 0.75f), transform.rotation).
                    GetComponent<Enemy_StrightBulletObj>().target = GameObject.FindGameObjectWithTag("Player").transform;
                slowed = false;
                break;

            case "playerEnter":
                //TriggerEvent (this, "attack");
                hasPos = true;
                break;

            case "playerExit":
                hasPos = false;
                break;

            case "attackThunder":
                TriggerEvent(this, "attackThunder", thunderGap, false);

                Vector2 rnd = Random.insideUnitCircle;
                eObject thunder = Instantiate(m_thunderPrefab, transform.position + new Vector3(rnd.x, 0, rnd.y) * 8.0f, Quaternion.identity).GetComponent<eObject>();
                thunder.TriggerEvent(thunder, "Activate", 2.5f);
                break;
        }
    }
}
