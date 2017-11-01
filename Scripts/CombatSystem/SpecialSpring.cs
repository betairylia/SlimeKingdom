using UnityEngine;
using System.Collections;

public class SpecialSpring : AttackAction
{
    public GameObject springBomb;

    public SpecialSpring(SlimeAttacker atker) : base(atker)
    {
        type = AttackActionType.SpecialSpring;
        timeLeft = 0.3f;
        timeCombo = 0.3f;
    }

    public override void Init()
    {
        base.Init();

        springBomb = GameObject.Instantiate(
            m_parent.springBomb, 
            m_transform.position + 0.569f * Vector3.up + m_transform.rotation * Vector3.forward * 0.9f, 
            m_transform.rotation);

        Physics.IgnoreCollision(m_parent.GetComponent<Collider>(), springBomb.GetComponent<Collider>());

        springBomb.GetComponent<Rigidbody>().velocity = m_parent.GetComponent<CharacterController>().velocity;
        //Debug.Log(m_parent.GetComponent<CharacterController>().velocity);
        springBomb.GetComponent<Rigidbody>().AddForce(180.0f * (m_transform.rotation * (Vector3.forward + 0.6f * Vector3.up)));
    }
}
