using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConsumeableItem
{
    public bool isItem;
    public ItemClass itemClass;
    public EquipmentClass equipmentClass;
    public int price;

    public ConsumeableItem(ItemClass item, int _price = 0)
    {
        isItem = true;
        itemClass = item;
        price = _price;
    }

    public ConsumeableItem(EquipmentClass equip, int _price = 0)
    {
        isItem = false;
        equipmentClass = equip;
        price = _price;
    }
};

//This one is SUUUUPERRRRR UGLY. (this class)
//but.
//emm.
public class ShopMenu : MonoBehaviour
{
    public int m_place, m_currentSelected;//0: village; 1: autumn day; 2: autumn night
    public int currentSelect;
    
    public ConsumeableItem[,] items = new ConsumeableItem[3,5];

    public Text itemName, itemDesc, itemPrice;

    public DialogShower dShower;
    public GameObject content, itemBox, equipBox;
    public Transform itemBoxesParent;

	// Use this for initialization
	void Start ()
    {
        content.SetActive(false);

        items[0, 0] = new ConsumeableItem(ItemClass.HealthPotion, 20);
        items[0, 1] = new ConsumeableItem(ItemClass.ManaPotion,30);
        items[0, 2] = new ConsumeableItem(ItemClass.SpeedUpPotion,50);
        items[0, 3] = new ConsumeableItem(EquipmentClass.Ribbon,30);
        items[0, 4] = new ConsumeableItem(EquipmentClass.SacredRelic,3800);

        items[1, 0] = new ConsumeableItem(ItemClass.HealthPotion,25);
        items[1, 1] = new ConsumeableItem(ItemClass.ManaPotion,35);
        items[1, 2] = new ConsumeableItem(ItemClass.RegenPotion,50);
        items[1, 3] = new ConsumeableItem(ItemClass.AttackUpPotion, 80);
        items[1, 4] = new ConsumeableItem(EquipmentClass.Maple,30);

        items[2, 0] = new ConsumeableItem(ItemClass.ManaPotion,20);
        items[2, 1] = new ConsumeableItem(ItemClass.RegenPotion,40);
        items[2, 2] = new ConsumeableItem(ItemClass.JumpUpPotion,100);
        items[2, 3] = new ConsumeableItem(ItemClass.GoldUpPotion, 200);
        items[2, 4] = new ConsumeableItem(EquipmentClass.Moonlight,30);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void Show(int place)
    {
        m_place = place;

        //refresh item boxes
        foreach(Transform tr in itemBoxesParent)
        {
            Destroy(tr.gameObject);
        }
        for(int i = 0; i < 5; i++)
        {
            if(items[m_place, i].isItem)
            {
                GameObject obj = Instantiate(itemBox, itemBoxesParent.lossyScale.x * (new Vector3(-180 + i * 90, 0)) + itemBoxesParent.position, Quaternion.identity, itemBoxesParent);
                obj.GetComponent<ItemBox>().m_class = items[m_place, i].itemClass;
                obj.GetComponent<ItemBox>().Index = i;
                obj.GetComponent<ItemBox>().IsNotInv();
                obj.GetComponent<Button>().onClick.AddListener(() => { this.Selected(obj.GetComponent<ItemBox>().Index); });

                if(i == 0)
                {
                    GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(obj);
                }
            }
            else
            {
                //TODO: make equipment box
            }
        }

        content.SetActive(true);
    }

    public void Selected(int id)
    {
        m_currentSelected = id;

        if(items[m_place, id].isItem)
        {
            itemName.text = InventoryDesc.itemName[(int)items[m_place, id].itemClass];
            itemDesc.text = InventoryDesc.itemDescText[(int)items[m_place, id].itemClass];
            itemPrice.text = "Price: " + items[m_place, id].price;
        }
    }

    public void BuyCurrent()
    {
        if (GameDataKeeper.GetSingleton().UseGold(items[m_place, m_currentSelected].price))
        {
            if(items[m_place, m_currentSelected].isItem)
            {
                GameDataKeeper.GetSingleton().itemCount[(int)items[m_place, m_currentSelected].itemClass]++;
            }
            else
            {
                GameDataKeeper.GetSingleton().equipmentExist[(int)items[m_place, m_currentSelected].equipmentClass] = true;
            }
        }
        else
        {
            //TODO: not enough money
        }
    }

    public void Back()
    {
        content.SetActive(false);
        dShower.clicked(1);
    }
}
