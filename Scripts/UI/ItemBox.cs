using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public ItemClass m_class;

    public int Index;
    public Text m_stackCount;
    public Image m_iconImage;
    bool isInv = true;

	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameDataKeeper.GetSingleton().itemCount[(int)m_class] > 0 || (!isInv))
        {
            Enable();
            if(isInv)
            {
                if (m_class <= ItemClass.ActivateCrystal)
                {
                    m_stackCount.gameObject.SetActive(false);
                }
                else
                {
                    m_stackCount.text = GameDataKeeper.GetSingleton().itemCount[(int)m_class].ToString();
                }
            }
            else
            {
                m_stackCount.text = GameDataKeeper.GetSingleton().itemCount[(int)m_class].ToString();

                if(m_class == ItemClass.None)
                {
                    m_stackCount.gameObject.SetActive(false);
                }
                else
                {
                    m_stackCount.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            Disable();
        }
	}

    public void Disable()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Button>().enabled = false;

        m_stackCount.gameObject.SetActive(false);
        m_iconImage.gameObject.SetActive(false);
    }

    public void Enable()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<Button>().enabled = true;

        m_stackCount.gameObject.SetActive(true);
        m_iconImage.gameObject.SetActive(true);
    }

    public void IsNotInv()
    {
        isInv = false;
    }
}
