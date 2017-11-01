using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : eObject
{
    public List<eObject> objectList = new List<eObject>();
    public GameObject targetMarker;

    public eObject m_slime;

	// Use this for initialization
	void Start ()
    {
        SetState("playerCollector");
        SetState("IgnoreProjectile");

        targetMarker = Instantiate(targetMarker, transform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Remove null objects
        objectList.RemoveAll((eObject e) => e == null ? true : false);

        //Sort by distence
        objectList.Sort((x, y) => (x.transform.position - transform.position).magnitude < (y.transform.position - transform.position).magnitude ? -1 : 0);

		if(objectList.Count > 0)
        {
            targetMarker.GetComponent<MeshRenderer>().enabled = true;

            Vector3 pos_tmp = objectList[0].transform.position;
            pos_tmp.y += objectList[0].GetComponent<Collider>().bounds.size.y + 0.3f;
            targetMarker.transform.position = pos_tmp;
        }
        else
        {
            targetMarker.GetComponent<MeshRenderer>().enabled = false;
        }
	}

    public override void OnObjectEnter(eObject obj)
    {
        base.OnObjectEnter(obj);

        if(obj.GetState("CollectableObject") > 0)
        {
            if (!objectList.Exists((eObject e) => e == obj ? true : false))
            {
                objectList.Add(obj);
            }
        }
        if(obj.GetState("autoCollect") > 0)
        {
            m_slime.TriggerEvent(obj, "pickUp");
        }
    }

    public override void OnObjectExit(eObject obj)
    {
        base.OnObjectExit(obj);

        if (obj.GetState("CollectableObject") > 0)
        {
            if (objectList.Exists((eObject e) => e == obj ? true : false))
            {
                objectList.Remove(obj);
            }
        }
    }
}
