using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject objItemBox;
    public InventoryDesc m_invDesc;

	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                GameObject obj = Instantiate(objItemBox, transform.lossyScale.x * (new Vector3(-270 + j * 90, 90 - i * 90)) + transform.position, Quaternion.identity, transform);
                obj.GetComponent<ItemBox>().m_class = (ItemClass)(i * 7 + j);
                obj.GetComponent<Button>().onClick.AddListener(() => { m_invDesc.Show(obj.GetComponent<ItemBox>().m_class); });
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
