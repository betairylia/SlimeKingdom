using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentTab : MonoBehaviour
{
    public static string[] equipmentNames =
    {
        "不知名的花朵",
        "藤蔓编织的花环",
        "浓缩枫树糖浆",
        "纯净冰晶",
        "流星碎片",
        "七彩护符",
        "神秘发光石",
        "破碎的梦之回忆",
        "皇冠",
        "四季之神的遗物",
        "蝴蝶结",
        "枫叶发饰",
        "月光石",
        "梦境的终点",
    };

    public static string[] equipmentDescs =
    {
        "在花海中盛开着的未名之花。" +
            "\n装备后，普通攻击的范围增加50%，" +
            "\n并变为花瓣特效。",
        "用藤蔓编织起来的花环。" +
            "\n装备后，敌人掉落的补给品将增加。",
        "一罐看起来很甜的食物（？）" +
            "\n装备后，将些许增加敌人掉落的金钱数量" +
            "\n并改变秋季形态的颜色。",
        "一块坚硬的冰晶，阳光直射也不会融化。" +
            "\n装备后，普通攻击附带减速效果。",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "一块谜一样的发光体，似乎承载着有关梦境的一切。" +
            "\n装备后，玩家可以穿过设置的不可通过区域（空气墙）。" +
            "\n由于是作弊器一样的存在，在使用上请注意。" +
            "\n可能会看到一些平常所见不到的风景，也可能会卡死。" +
            "\n即使在卡死时存档，重新读档可以返回初始位置。",
    };

    public static string[] equipmentHints =
    {
        "尚未获得。提示：花海之上",
        "尚未获得。提示：夏日中的冰凉",
        "尚未获得。提示：秋叶无法停留的地方",
        "尚未获得。提示：暴风雪的尽头",
        "尚未获得。提示：通往愿望的道路",
        "尚未获得。提示：秋夜中的繁星",
        "尚未获得。提示：四散各地的秘密",
        "尚未获得。提示：当勇者罢工回家之后",
        "尚未获得。提示：长者的证明",
        "尚未获得。提示：你得...特别有钱。",
        "尚未获得。提示：你得有钱",
        "尚未获得。提示：你得有钱",
        "尚未获得。提示：你得有钱",
        "尚未获得。提示：夜空下最后的秘密",
    };

    public GameObject objEquipBox;
    public EquipmentClass m_currentSelected;
    public Text text_equipmentName, text_equipmentDesc;

    public Button btn_equip, btn_dismount;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                GameObject obj = Instantiate(objEquipBox, transform.lossyScale.x * (new Vector3(-270 + j * 90, 100 - i * 90)) + transform.position, Quaternion.identity, transform);
                obj.GetComponent<EquipBox>().m_class = (EquipmentClass)(i * 7 + j);
                obj.GetComponent<Button>().onClick.AddListener(() => { this.SelectEquipment(obj.GetComponent<EquipBox>().m_class); });
            }
        }

        btn_dismount.gameObject.SetActive(false);
        btn_equip.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void SelectEquipment(EquipmentClass selected)
    {
        if(selected == GameDataKeeper.GetSingleton().currentEquipment)
        {
            btn_dismount.gameObject.SetActive(true);
        }
        else
        {
            btn_dismount.gameObject.SetActive(false);
        }

        m_currentSelected = selected;

        if (GameDataKeeper.GetSingleton().equipmentExist[(int)m_currentSelected])
        {
            text_equipmentName.text = equipmentNames[(int)m_currentSelected];
            text_equipmentDesc.text = equipmentDescs[(int)m_currentSelected];
            btn_equip.gameObject.SetActive(true);
        }
        else
        {
            text_equipmentName.text = "？？？";
            text_equipmentDesc.text = equipmentHints[(int)m_currentSelected];
            btn_equip.gameObject.SetActive(false);
        }
    }

    public void EquipCurrent()
    {
        GameDataKeeper.GetSingleton().Equip(m_currentSelected);
        btn_dismount.gameObject.SetActive(true);
    }

    public void Dismount()
    {
        GameDataKeeper.GetSingleton().Equip(EquipmentClass.None);
        btn_dismount.gameObject.SetActive(false);
    }
}
