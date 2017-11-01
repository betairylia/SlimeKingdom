using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public static string[] mainQuestLogHint =
    {
        "BAD STRING",
        "探索一下周围吧！",             //NewStart_Tutorial,                      //  新游戏开始，需要显示教程
        "向着前方前进吧！",//Tutorial_Finished,                      //  教程结束，门打开（但是不一定离开初始区域）
        "去找村中的长老了解情况",//AimFor_VillageLord,                     //  准备去见长老
        "前往<花海>",//AimFor_SpringFlowerSea,                 //  结束对话，明确要去花海
        "前往<花海>",//Solved_SpringCave,                      //  通过了春季洞窟
        "返回村庄，向长老报告春之精灵",//GoingBackToVillage_WithSpringSprite,    //  结束春精灵剧情回村子
        "前往古树，解救夏之精灵！",//AimFor_SummerGreatTree,                 //  结束对话，明确要去古树
        "调查神秘的装置",//SummerForest1_FindBridge,               //  目标：找到桥
        "在去古树之前，先前往满秋神社",//AimFor_AutumnShrine,                    //  桥出来，巫女已经离开，明确要去神社
        "寻找神社的祀具和5个苹果",//AutumnShrine_FindItem,                  //  准备去找东西（满秋神社）
        "找齐了，去找大巫女报告",//AutumnShrine_ItemFoundALL,              //  东西找齐了
        "和秋季精灵对话！",//AutumnShrine_GoMeetAutumn,              //  去见秋季精灵！
        "获得了水晶，从<夏季森林>前往古树",//AimFor_SummerGreatTree_After,           //  再次准备前往古树
        "救出夏季精灵！\n已启动的传送点：绿、蓝",//SummerGreatTree_TP1,                    //  古树区域 蓝色传送点
        "救出夏季精灵！\n已启动的传送点：绿、蓝、黄",//SummerGreatTree_TP2,                    //  古树区域 黄色传送点
        "救出夏季精灵！\n已启动的传送点：绿、蓝、黄、红",//SummerGreatTree_TP3,                    //  古树区域 红色传送点
        "击败了魔王！...的幻影，和夏季精灵对话吧",//SummerGreatTree_BossDead,               //击破Boss，准备和夏季精灵对话
        "返回村庄，向长老报告",//Solved_SummerGreatTree,                 //解决夏季古树区域，古树区回到原来的样子，准备回村子
        "",//AimFor_WinterRoad,                      //  完成对话，准备前往冬季小径
        "",//Battle_WinterSprite,                    //  完成与狂化的冬季精灵的对话，战斗开始（存档点1）
        "",//Solved_WinterSprite,                    //  解决了狂化的冬季精灵
        "",//AimFor_SeasonsTemple,                   //  准备前往四季神殿
        "",//SeasonsTemple_GateOpened,               //四季神殿入口打开
        "",//SeasonsTemple_PortalOpened,             //四季神殿Boss入口打开
        "",//SeasonsTemple_BossPhase1,               //对话完毕，进入P1
        "",//SeasonsTemple_BossPhase2,               //进入P2（存档点1）
        "",//SeasonsTemple_BossPhase3,               //进入P3（存档点2）
        "",//Solved_SeasonsTemple,                   //解决了Boss，该从四季神殿的屋顶掉下来了
        "",//AimFor_Village,                         //和芙蕾露对话完毕，回村子
    };

    public Text formShower, crystalShower, healthShower, timerShower, manaShower, goldShower;
    public Text mainQuestLogText, subQuestLogText;
    public Image blackScreen;
    public float timer;
    public DialogShower dShower;
    public GameObject gameMenu, openMenuFirstSelect;
    public InventoryDesc m_invDesc;

    public ItemBox shortcut1, shortcut2, shortcut3;

    public float barLenth;
    public RawImage bossHealth, bossMana;
    public GameObject bossHealthStatusBar;

    float blackSpeed = 0, blackRatio = 0;
    bool isBlackScreenOn = false;

    // Use this for initialization
    void Start()
    {
        blackScreen.enabled = true;
        blackScreen.gameObject.SetActive(false);

        gameMenu.SetActive(false);

        HideBossBar();

        //shortcuts should not disappear
        shortcut1.IsNotInv();
        shortcut2.IsNotInv();
        shortcut3.IsNotInv();

        //Register events for item shortcuts
        shortcut1.GetComponent<Button>().onClick.AddListener(() => { GameDataKeeper.GetSingleton().UseItem(shortcut1.m_class); });
        shortcut2.GetComponent<Button>().onClick.AddListener(() => { GameDataKeeper.GetSingleton().UseItem(shortcut2.m_class); });
        shortcut3.GetComponent<Button>().onClick.AddListener(() => { GameDataKeeper.GetSingleton().UseItem(shortcut3.m_class); });
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (isBlackScreenOn)
        {
            blackRatio += blackSpeed * Time.deltaTime;
        }
        else
        {
            blackRatio -= blackSpeed * Time.deltaTime;
        }

        blackRatio = Mathf.Clamp(blackRatio, 0.0f, 1.0f);
    }

    public bool IsPlayerUnfreezeable()
    {
        if(dShower.IsDialogShowing())
        {
            return false;
        }
        return true;
    }

    private void OnGUI()
    {
        switchForm(GameDataKeeper.GetSingleton().currentForm);
        setCrystalCount(GameDataKeeper.GetSingleton().crystalCount);
        setHealth((int)GameDataKeeper.GetSingleton().currentHealth);
        setMana((int)GameDataKeeper.GetSingleton().currentMagic);
        setGold(GameDataKeeper.GetSingleton().currentGold);

        //TEST ONLY TEST ONLY TEST ONLY
        if (timer < 0)
        {
            timer = 0;
        }

        timerShower.text = "Timer: " + timer;
        //TEST ONLY TEST ONLY TEST ONLY

        blackScreen.canvasRenderer.SetAlpha(blackRatio);
        if(blackRatio == 0.0f && isBlackScreenOn == false)
        {
            blackScreen.gameObject.SetActive(false);
        }

        //Update Log Text
        mainQuestLogText.text = "主要目标:\n    <size=15>" + mainQuestLogHint[(int)GameDataKeeper.GetSingleton().currentProcess] + "</size>";

        //Update shortcut box
        shortcut1.GetComponent<ItemBox>().m_class = GameDataKeeper.GetSingleton().itemOnKey[0];
        shortcut2.GetComponent<ItemBox>().m_class = GameDataKeeper.GetSingleton().itemOnKey[1];
        shortcut3.GetComponent<ItemBox>().m_class = GameDataKeeper.GetSingleton().itemOnKey[2];
    }

    public void switchForm(SlimeForm formId)
    {
        string tmpStr = "Form: ";
        for (int i = 0; i < 5; i++)
        {
            if (i == (int)formId)
            {
                tmpStr += "<size=100>" + (i + 1) + "</size>";
            }
            else
            {
                tmpStr += "" + (i + 1);
            }
        }
        formShower.text = tmpStr;
    }

    public void setCrystalCount(int count)
    {
        crystalShower.text = "Crystals: " + count;
    }

    public void setHealth(int health)
    {
        healthShower.text = "HP: " + health;
    }

    public void setMana(int mana)
    {
        manaShower.text = "SP: " + mana;
    }

    public void setGold(int gold)
    {
        goldShower.text = "Gold: " + gold + "G";
    }

    public void ShowBossBar()
    {
        bossHealthStatusBar.SetActive(true);
    }

    public void HideBossBar()
    {
        bossHealthStatusBar.SetActive(false);
    }

    public void SetBossHealth(float healthPercentage)
    {
        bossHealth.rectTransform.localScale = new Vector3(healthPercentage, 1.0f, 1.0f);
    }

    public void SetBossMana(float manaPercentage)
    {
        bossMana.rectTransform.localScale = new Vector3(manaPercentage, 1.0f, 1.0f);
    }

    //Instantly.
    public void BlackScreenOn()
    {
        blackScreen.gameObject.SetActive(true);
        blackRatio = 1.0f;
        blackScreen.canvasRenderer.SetAlpha(blackRatio);
        isBlackScreenOn = true;
    }

    public void BlackScreenOn(float time)
    {
        blackScreen.gameObject.SetActive(true);
        blackSpeed = 1.0f / time;
        isBlackScreenOn = true;
    }

    //Instantly.
    public void BlackScreenOff()
    {
        blackScreen.gameObject.SetActive(false);
        blackRatio = 0.0f;
        blackScreen.canvasRenderer.SetAlpha(blackRatio);
        isBlackScreenOn = false;
    }

    public void BlackScreenOff(float time)
    {
        blackSpeed = 1.0f / time;
        isBlackScreenOn = false;
    }

    public GameObject INV_DEBUG;

    public void Tutorial_WaitForCloseInventory()
    {
        //Debug test
        GameObject tmp = Instantiate(INV_DEBUG, transform.parent);
        tmp.GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("4_SetGoal").GetComponent<eObject>().TriggerEvent(GameObject.Find("4_SetGoal").GetComponent<eObject>(), "Activate");
            Destroy(tmp); });
        //Debug test
    }

    public void ShowMenu()
    {
        //prevent the menu be opened while player is frozen
        if(!IsPlayerUnfreezeable())
        {
            return;
        }

        m_isMenuShown = true;
        gameMenu.SetActive(true);

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(openMenuFirstSelect);
    }

    public void CloseMenu()
    {
        m_invDesc.Hide();
        m_isMenuShown = false;
        gameMenu.SetActive(false);
    }

    bool m_isMenuShown = false;
    public bool isMenuShown()
    {
        return m_isMenuShown;
    }

    public void SaveGame()
    {
        GameDataKeeper.GetSingleton().SaveGame("save.dat");
    }

    public void LoadGame()
    {
        GameDataKeeper.GetSingleton().LoadGame("save.dat");
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
    }
}
