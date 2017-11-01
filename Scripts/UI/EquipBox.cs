using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBox : MonoBehaviour
{
    public EquipmentClass m_class;

    public int Index;
    public Image m_iconImage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameDataKeeper.GetSingleton().equipmentExist[(int)m_class])
        {
            //TODO
        }
        else
        {
            //TODO
        }
    }

    public void Disable()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Button>().enabled = false;

        m_iconImage.gameObject.SetActive(false);
    }

    public void Enable()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<Button>().enabled = true;

        m_iconImage.gameObject.SetActive(true);
    }
}
