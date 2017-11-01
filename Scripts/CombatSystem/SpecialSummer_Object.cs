using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSummer_Object : eObject
{
    public float startEulerX = -15, targetEulerX = 0;
    public float startScaleZ = 2, targetScaleZ = 10;
    public float totalTime = 0.5f;
    public AnimationCurve curve;
    float m_speed, nowTime;
    Quaternion rot_backUp;

    Slime m_slime;

    public Transform m_whip, m_targetCatcher;

	// Use this for initialization
	void Start ()
    {
        m_speed = 1.0f / totalTime;
        nowTime = 0.0f;
        rot_backUp = transform.rotation;
	}

    public void SetParentSlime(Slime slime)
    {
        m_slime = slime;
        m_whip.GetComponent<SpecialSummer_Attacker>().SetParentSlime(m_slime);
    }
	
	// Update is called once per frame
	void Update ()
    {
        nowTime += m_speed * Time.deltaTime;
        float curvePos = curve.Evaluate(nowTime);

        if(nowTime >= 0.9f)
        {
            m_whip.GetComponent<SpecialSummer_Attacker>().EnableHook();
        }

        if (nowTime >= 1.3f)
        {
            Destroy(gameObject);
        }

        transform.rotation = rot_backUp * Quaternion.Euler(Vector3.right * Mathf.Lerp(startEulerX, targetEulerX, curvePos));
        m_whip.localScale = new Vector3(m_whip.localScale.x, m_whip.localScale.y, Mathf.Lerp(startScaleZ, targetScaleZ, curvePos));
        m_whip.localPosition = Vector3.forward * (1.0f + 0.5f * Mathf.Lerp(startScaleZ, targetScaleZ, curvePos));

        m_targetCatcher.position = transform.position + m_whip.rotation * (Vector3.forward * (m_whip.localScale.z + 1.0f));
	}
}
