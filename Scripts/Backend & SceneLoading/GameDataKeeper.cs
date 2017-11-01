using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

[Serializable]
public enum GameMainProcessState
{
                                            //>>有两个箭头表示这是下一个要做的事情（TODOs）
    None,                                   //  前面有空格表示可到达（可从上一个阶段转到下一个阶段）
    NewStart_Tutorial,                      //  新游戏开始，需要显示教程
    Tutorial_Finished,                      //  教程结束，门打开（但是不一定离开初始区域）
    AimFor_VillageLord,                     //  准备去见长老
    AimFor_SpringFlowerSea,                 //  结束对话，明确要去花海
    Solved_SpringCave,                      //  通过了春季洞窟
    GoingBackToVillage_WithSpringSprite,    //  结束春精灵剧情回村子
    AimFor_SummerGreatTree,                 //  结束对话，明确要去古树
    SummerForest1_FindBridge,               //  目标：找到桥
    AimFor_AutumnShrine,                    //  桥出来，巫女已经离开，明确要去神社
    AutumnShrine_FindItem,                  //  准备去找东西（满秋神社）
    AutumnShrine_ItemFoundALL,              //  东西找齐了
    AutumnShrine_GoMeetAutumn,              //  去见秋季精灵！
    AimFor_SummerGreatTree_After,           //  再次准备前往古树
    SummerGreatTree_TP1,                    //  古树区域 蓝色传送点
    SummerGreatTree_TP2,                    //  古树区域 黄色传送点
    SummerGreatTree_TP3,                    //  古树区域 红色传送点
    SummerGreatTree_BossDead,               //击破Boss，准备和夏季精灵对话
    Solved_SummerGreatTree,                 //解决夏季古树区域，古树区回到原来的样子，准备回村子
    AimFor_WinterRoad,                      //  完成对话，准备前往冬季小径
    Battle_WinterSprite,                    //  完成与狂化的冬季精灵的对话，战斗开始（存档点1）
    Solved_WinterSprite,                    //  解决了狂化的冬季精灵
    AimFor_SeasonsTemple,                   //  准备前往四季神殿
    SeasonsTemple_GateOpened,               //四季神殿入口打开
    SeasonsTemple_PortalOpened,             //四季神殿Boss入口打开
    SeasonsTemple_BossPhase1,               //对话完毕，进入P1
    SeasonsTemple_BossPhase2,               //进入P2（存档点1）
    SeasonsTemple_BossPhase3,               //进入P3（存档点2）
    Solved_SeasonsTemple,                   //解决了Boss，该从四季神殿的屋顶掉下来了
    AimFor_Village,                         //和芙蕾露对话完毕，回村子
};

//Ugly enums.
//but.
//emm..

[Serializable]
public enum ItemClass
{
    //Key Items
    HeartStone,
    FlowerBucket,
    SpringSoul,
    SummerSoul,
    AutumnSoul,
    WinterSoul,
    ActivateCrystal,

    //Potions
    HealthPotion,
    ManaPotion,
    SpeedUpPotion,
    AttackUpPotion,
    RegenPotion,
    GoldUpPotion,
    JumpUpPotion,

    //Other Items
    Apple,
    Shroom,
    WTF1,//TODO
    TeleportStone,
    ShrineTool,
    MemoryOfStone,
    CrystalOfFriendship,

    None
};

[Serializable]
public enum EquipmentClass
{
    SpringFlower,
    SummerBlabla,//TODO
    AutumnBlabla,//TODO
    WinterBlabla,//TODO
    ShootingStar,
    Colorful,
    SecretsOfStone,
    PastingDream,
    King,
    SacredRelic,
    Ribbon,
    Maple,
    Moonlight,
    EndOfDream,
    None
};

[Serializable]
public enum BuffClass
{
    RegenPotion,
    Shroom_SlowAndPoisoned,
    Shroom_NoMana,
    GreatTreeBoss_baseAttack_areaEffectSlow,
};

[Serializable]
public class Buff
{
    public BuffClass buffClass;
    public float timeRemain, timeTotal;
    public GameObject buffDisplayer;

    public void UpdateTime(float dt)
    {
        if(timeTotal > 0)
        {
            timeRemain -= dt;
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(buffDisplayer);
    }
};

[Serializable]
public class GameDataKeeper
{
    public string targetSceneName, prevSceneName;
    public int crystalCount;// { private set; get; }
    public float currentHealth, maxHealth, currentMagic, maxMagic;
    public SlimeForm currentForm;// { private set; get; }
    public GameMainProcessState currentProcess;// { private set; get; }

    public int currentGold;// { private set; get; }
    public int[] itemCount;// { set; get; }
    public bool[] equipmentExist;// { set; get; }
    public EquipmentClass currentEquipment;// { private set; get; }
    public ItemClass[] itemOnKey;// { private set; get; }

    public List<Buff> playerBuffList;// { private set; get; }

    public bool isAutumnNightVisited;
    public bool sub_ColorfulClear, sub_ShootingStarClear, sub_StonesClear, sub_MemoriesClear;
    public bool[] isStoneVisited = new bool[12];

    private Dictionary<string, bool> sceneObjectsActivated = new Dictionary<string, bool>();

    private static GameDataKeeper singleton;
    public static GameDataKeeper GetSingleton()
    {
        if (singleton == null)
            singleton = new GameDataKeeper();
        return singleton;
    }

    private GameDataKeeper()
    {
        Debug.Log("Keeper CTOR");

        prevSceneName = "Menu";

        itemCount = new int[22];
        equipmentExist = new bool[14];
        itemOnKey = new ItemClass[3];

        for (int i = 0; i < 22; i++)
        {
            itemCount[i] = 0;
        }
        for (int i = 0; i < 14; i++)
        {
            equipmentExist[i] = true;
        }

        itemOnKey[0] = itemOnKey[1] = itemOnKey[2] = ItemClass.None;

        //TEST ONLY TEST ONLY TEST ONLY
        targetSceneName = "StartArea";

        currentHealth = 80;
        maxHealth = 80;

        currentMagic = 100;
        maxMagic = 100;

        currentGold = 1000;

        currentProcess = GameMainProcessState.NewStart_Tutorial;

        itemCount[2] = 1;
        itemCount[3] = 1;
        itemCount[4] = 1;
        itemCount[5] = 1;
        //TEST ONLY TEST ONLY TEST ONLY

        //Give player the heart stone at first
        itemCount[(int)ItemClass.HeartStone] = 1;
        currentEquipment = EquipmentClass.None;

        //Create buff list
        playerBuffList = new List<Buff>();

        //Sub quests etc.
        sub_ColorfulClear = sub_ShootingStarClear = sub_StonesClear = sub_MemoriesClear = false;
        isAutumnNightVisited = false;
    }

    public bool UseGold(int amount)
    {
        if(currentGold >= amount)
        {
            currentGold -= amount;
            return true;
        }
        return false;
    }

    public void UseMagic(float amount)
    {
        currentMagic -= amount;
        if(currentMagic <= 0)
        {
            currentMagic = 0;
        }
    }

    public void GainMagic(float amount)
    {
        currentMagic += amount;
        if(currentMagic > maxMagic)
        {
            currentMagic = maxMagic;
        }
    }

    public void GetGold(int amount)
    {
        currentGold += amount;
    }

    public void GetItem(string itemName)
    {
        ItemClass tmp_item = ItemClass.HealthPotion;
        EquipmentClass tmp_equip = EquipmentClass.Ribbon;
        bool isItem = true;

        switch(itemName)
        {
            //TODO: use hash tables instead
            case "SpringSoul":
                tmp_item = ItemClass.SpringSoul;
                break;
            case "SummerSoul":
                tmp_item = ItemClass.SummerSoul;
                break;
            case "AutumnSoul":
                tmp_item = ItemClass.AutumnSoul;
                break;
            case "WinterSoul":
                tmp_item = ItemClass.WinterSoul;
                break;
            case "ActivateCrystal":
                tmp_item = ItemClass.ActivateCrystal;
                break;
            case "RegenPotion":
                tmp_item = ItemClass.RegenPotion;
                break;
            case "Shroom":
                tmp_item = ItemClass.Shroom;
                break;
            case "Apple":
                tmp_item = ItemClass.Apple;
                break;
            case "ShrineTool":
                tmp_item = ItemClass.ShrineTool;
                break;
            case "SpringFlower":
                tmp_equip = EquipmentClass.SpringFlower;
                isItem = false;
                break;
            case "Crystal":
                GetCrystal();
                return;
            default:
                return;
        }

        if(isItem)
        {
            itemCount[(int)tmp_item]++;
            GameObject.Find("ItemGetHint").GetComponent<ItemHintArea>().AddItemHint(new ConsumeableItem(tmp_item));
        }
        else
        {
            equipmentExist[(int)tmp_equip] = true;
            GameObject.Find("ItemGetHint").GetComponent<ItemHintArea>().AddItemHint(new ConsumeableItem(tmp_equip));
        }

        //check item state
        if ( currentProcess == GameMainProcessState.AutumnShrine_FindItem &&
            itemCount[(int)ItemClass.Apple] >= 5 && 
            itemCount[(int)ItemClass.ShrineTool] >= 1)
        {
            SetMainProcess(GameMainProcessState.AutumnShrine_ItemFoundALL);
        }
    }

    public void UseItem(ItemClass item)
    {
        if(item == ItemClass.None)
        {
            return;
        }
        //TODO
        if(itemCount[(int)item] > 0)
        {
            itemCount[(int)item]--;

            switch(item)
            {
                case ItemClass.RegenPotion:
                    AddBuff(BuffClass.RegenPotion, 30.0f);
                    break;
            }
        }
    }

    public void AddBuff(BuffClass buff, float time)
    {
        Buff buf = new Buff();
        buf.buffClass = buff;
        buf.timeTotal = time;
        buf.timeRemain = time;
        buf.buffDisplayer = GameObject.Find("BuffList").GetComponent<BuffList>().GetBuffDisplayer(buf);

        playerBuffList.Add(buf);
    }

    public void AddBuff(BuffClass buff)
    {
        Buff buf = new Buff();
        buf.buffClass = buff;
        buf.timeTotal = 0;
        buf.timeRemain = 1;
        buf.buffDisplayer = GameObject.Find("BuffList").GetComponent<BuffList>().GetBuffDisplayer(buf);

        playerBuffList.Add(buf);
    }

    public bool HasBuff(BuffClass targetClass)
    {
        foreach(Buff buf in playerBuffList)
        {
            if(buf.buffClass == targetClass)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveBuff(BuffClass targetClass)
    {
        for (int i = 0; i < playerBuffList.Count; i++)
        {
            if (playerBuffList[i].buffClass == targetClass)
            {
                playerBuffList[i].Destroy();
                playerBuffList.Remove(playerBuffList[i]);
                i--;
            }
        }
    }

    public void UpdateBuff(float dt)
    {
        for(int i = 0; i < playerBuffList.Count; i++)
        {
            playerBuffList[i].UpdateTime(dt);

            if(playerBuffList[i].timeRemain < 0)
            {
                playerBuffList[i].Destroy();
                playerBuffList.Remove(playerBuffList[i]);
                i--;
            }
        }
    }

    public void Equip(EquipmentClass equipment)
    {
		//TODO  
		currentEquipment = equipment;
    }

    public void SwitchForm(SlimeForm form)
    {
        switch(form)
        {
            case SlimeForm.Spring:
                if(itemCount[(int)ItemClass.SpringSoul] > 0)
                {
                    currentForm = form;
                }
                break;
            case SlimeForm.Summer:
                if (itemCount[(int)ItemClass.SummerSoul] > 0)
                {
                    currentForm = form;
                }
                break;
            case SlimeForm.Autumn:
                if (itemCount[(int)ItemClass.AutumnSoul] > 0)
                {
                    currentForm = form;
                }
                break;
            case SlimeForm.Winter:
                if (itemCount[(int)ItemClass.WinterSoul] > 0)
                {
                    currentForm = form;
                }
                break;
            default:
                currentForm = form;
                break;
        }
    }

    public void GetCrystal()
    {
        crystalCount++;
    }

    public bool UseCrystal()
    {
        if (crystalCount > 0)
        {
            crystalCount--;
            return true;
        }

        return false;
    }

    public void SetMainProcess(GameMainProcessState process)
    {
        if(process == GameMainProcessState.None || process <= currentProcess)
        {
            return;
        }

        currentProcess = process;

        foreach(NPCBase npc in GameObject.FindObjectsOfType<NPCBase>())
        {
            npc.CheckState();
        }
    }

    public void ForceSetMainProcess(GameMainProcessState process)
    {
        if (process == GameMainProcessState.None)
        {
            return;
        }

        currentProcess = process;

        foreach (NPCBase npc in GameObject.FindObjectsOfType<NPCBase>())
        {
            npc.CheckState();
        }
    }

    public void SetKey(int key, ItemClass cls)
    {
        itemOnKey[key] = cls;
    }

    public bool GetObjectState(string name)
    {
        if(sceneObjectsActivated.ContainsKey(name))
        {
            return sceneObjectsActivated[name];
        }
        else
        {
            sceneObjectsActivated.Add(name, false);
            return false;
        }
    }

    public bool SetObjectState(string name, bool state)
    {
        if (sceneObjectsActivated.ContainsKey(name))
        {
            sceneObjectsActivated[name] = state;
            return true;
        }
        else
        {
            sceneObjectsActivated.Add(name, state);
            return false;
        }
    }

    public void SaveGame(string path)
    {
        Debug.Log("Game saved as: " + JsonUtility.ToJson(this));

        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this));
    }

    public void LoadGame(string path)
    {
        singleton = JsonUtility.FromJson<GameDataKeeper>(File.ReadAllText(path));
    }
}
