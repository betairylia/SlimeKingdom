using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffList : MonoBehaviour
{
    public GameObject buffBlock;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = i * 110 * Vector3.down;
        }
	}

    public GameObject GetBuffDisplayer(Buff buf)
    {
        int count = transform.childCount;

        GameObject obj = Instantiate(buffBlock, count * 110 * Vector3.down * transform.lossyScale.x, Quaternion.identity, transform);
        obj.GetComponent<BuffBlock>().m_parent = buf;

        return obj;
    }
}
