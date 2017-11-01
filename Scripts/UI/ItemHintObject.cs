using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHintObject : MonoBehaviour
{
    public Transform content;
    public ConsumeableItem item;
    float timeNow;

    public Image itemIcon;
    public Text itemName;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 3.0f);
        timeNow = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeNow += Time.deltaTime;

        if(timeNow < 0.4f)
        {
            //in
            content.localPosition = (0.4f - timeNow) / 0.4f * 300.0f * Vector3.right;
        }
        if(timeNow > 2.6f)
        {
            //out
            content.localPosition = (timeNow - 2.6f) / 0.4f * 300.0f * Vector3.right;
        }

        if(item != null)
        {
            if (item.isItem)
            {
                itemName.text = InventoryDesc.itemName[(int)item.itemClass];
            }
            else
            {
                itemName.text = "（装备品）";
            }
        }
	}
}
