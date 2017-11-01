using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffBlock : MonoBehaviour
{
    public static string[] BuffTitle =
    {   
        "缓缓回复",
        "中毒！",
        "魔力流失！",
        "攻击减速",//GreatTreeBoss_baseAttack_areaEffectSlow,
    };

    public static string[] BuffDesc =
    {
        "持续回复体力。",
        "持续损失体力并被减速。",
        "魔力被清空。",
        "造成伤害并减速。快出去！",
    };

    public Buff m_parent;
    public Text title, desc, timer;
    public Image icon, timerBar;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        title.text = BuffTitle[(int)m_parent.buffClass];
        desc.text = BuffDesc[(int)m_parent.buffClass];
        if(m_parent.timeTotal > 0)
        {
            timer.text = (int)(m_parent.timeRemain / 60.0f) + " : " + (int)(m_parent.timeRemain) % 60;
            timerBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_parent.timeRemain / m_parent.timeTotal * 300.0f);
        }
        else
        {
            timer.text = "";
            timerBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300.0f);
        }
	}
}
