using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHintArea : MonoBehaviour
{
    public GameObject hintObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).localPosition = (transform.childCount - 1 - i) * 80.0f * Vector3.up;

            if (i < transform.childCount - 3)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void AddItemHint(ConsumeableItem item)
    {
        ItemHintObject obj = Instantiate(
            hintObject, 
            transform.lossyScale.x * (new Vector3(300.0f, 0.0f)) + transform.position, 
            Quaternion.identity, 
            transform).GetComponent<ItemHintObject>();
        obj.item = item;
    }
}
