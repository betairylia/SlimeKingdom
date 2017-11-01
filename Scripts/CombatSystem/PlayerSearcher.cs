using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearcher : eObject
{
    //public float searchRange = 10;
    public eObject parent;
    public bool isHitPlayer;

    CharacterController playerCharController;
    _slime_SphereCollider _s_playerCollider;
    Collider[] m_collider;
    _slime_SphereCollider[] m_s_collider;

    // Use this for initialization
    void Start ()
    {
        SetState("IgnoreProjectile");
        //    gameObject.GetComponent<SphereCollider>().radius = searchRange;
        //    gameObject.GetComponent<SphereCollider>().isTrigger = true;

        playerCharController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _s_playerCollider = new _slime_SphereCollider(playerCharController.transform.rotation * playerCharController.center + playerCharController.transform.position, playerCharController.radius);
        m_collider = gameObject.GetComponents<SphereCollider>();

        int i = 0;
        m_s_collider = new _slime_SphereCollider[m_collider.Length];
        foreach (SphereCollider c in m_collider)
        {
            m_s_collider[i] = new _slime_SphereCollider(c.transform.rotation * c.center + c.transform.position, c.radius);
            c.enabled = false;
            i++;
        }

        isHitPlayer = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        int i = 0;
        bool hitflag = false;

        _s_playerCollider.SetProps(playerCharController.transform.rotation * playerCharController.center + playerCharController.transform.position, playerCharController.radius);
        foreach (SphereCollider c in m_collider)
        {
            //TODO: cast
            m_s_collider[i].SetProps(c.transform.rotation * c.center + c.transform.position, c.radius);

            if(m_s_collider[i].Intersects(_s_playerCollider))
            {
                hitflag = true;
            }

            i++;
        }

        if(hitflag && !isHitPlayer)
        {
            //Player enter
            TriggerEvent(parent, "playerEnter " + Toolbox.ToRawString(playerCharController.transform.position), 0, false);
        }
        else if (!hitflag && isHitPlayer)
        {
            //Player exit
            TriggerEvent(parent, "playerExit " + Toolbox.ToRawString(playerCharController.transform.position), 0, false);
        }
        else if (hitflag && isHitPlayer)
        {
            //Player stay
            TriggerEvent(parent, "playerPos " + Toolbox.ToRawString(playerCharController.transform.position), 0, false);
        }
        else
        {
            //Nothing to do
        }

        isHitPlayer = hitflag;
	}
}
