using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDesc : MonoBehaviour
{
    public static string[] itemName =
    {
        "心形石",
        "花篮",
        "繁花之种",
        "夏季藤蔓",
        "枫树糖浆",
        "纯净冰晶",
        "升降梯激活水晶",

        "体力药水",
        "魔力药水",
        "加速药水",
        "攻击药水",
        "缓缓回复药水",
        "贪婪药水",
        "跳跃药水",

        "秋天的苹果",
        "秋天的蘑菇",
        "？？？",
        "传送石",
        "神社的祀具",
        "石碑的记忆",
        "朋友证明"
    };

    public static string[] itemDescText =
    {
        "不知道什么时候跑进背包里的奇怪的石头，散发着温暖的光芒。",
        "为邻居家的女儿芙蕾露准备生日礼物时用到的花篮，仿佛能从中看到她的笑脸。",
        "繁花之种",
        "夏季藤蔓",
        "枫树糖浆",
        "纯净冰晶",
        "升降梯激活水晶",

        "饮用，立即回复50点体力。",
        "饮用，魔法不会被消耗。持续30秒。",
        "饮用，移动速度提升40%。持续20秒。",
        "饮用，攻击力提升50%。持续2分钟。",
        "饮用，在20秒内回复100点体力。",
        "饮用，击倒敌人后可以获得250%的金钱，持续5分钟",
        "饮用，跳跃力提升，持续2分钟",

        "红透了的苹果。回复体力30点，并在接下来的15秒内额外回复50点体力。",
        "在秋天遍地可见的蘑菇。吃了会有意想不到的效果？！",
        "？？？",
        "散发着古怪光芒的石头。使用后可以传送，根据季节形态会有不同的效果。",
        "神社的祀具",
        "石碑的记忆",
        "击败魔物掉落的绿色结晶。\n\"我们...可以一起玩吧？\""
    };

    public GameObject content, backBtn, optionalContent;
    public Text textDesc, textName;
    GameObject prevSelected;
    ItemClass m_class;

    // Use this for initialization
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(ItemClass itemclass)
    {
        m_class = itemclass;

        content.SetActive(true);
        optionalContent.SetActive(true);

        prevSelected = GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(backBtn);

        textName.text = itemName[(int)m_class];
        textDesc.text = itemDescText[(int)m_class];

        if (m_class <= ItemClass.ActivateCrystal)
        {
            optionalContent.SetActive(false);
        }
    }

    public void Hide()
    {
        content.SetActive(false);

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(prevSelected);
    }

    public void Use()
    {
        GameDataKeeper.GetSingleton().UseItem(m_class);
        Hide();
    }

    public void SetKey(int key)
    {
        GameDataKeeper.GetSingleton().SetKey(key, m_class);
    }
}
