using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class projectile : eObject
{
    public projectile()
    {
        SetState("projectile");
    }

    //passive part
    //NB. When using OnTriggerXXX on other classes which has inherited eObject class, call base.OnTriggerXXX(obj) first.
    public virtual void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.GetComponent<eObject>() != null)
        {
            if (obj.gameObject.GetComponent<eObject>().GetState("IgnoreProjectile") > 0)
            {
                return;
            }
            OnObjectEnter(obj.gameObject.GetComponent<eObject>());
        }
    }

    public virtual void OnTriggerStay(Collider obj)
    {
        if (obj.gameObject.GetComponent<eObject>() != null)
        {
            if (obj.gameObject.GetComponent<eObject>().GetState("IgnoreProjectile") > 0)
            {
                return;
            }
            OnObjectStay(obj.gameObject.GetComponent<eObject>());
        }
    }

    public virtual void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.GetComponent<eObject>() != null)
        {
            if (obj.gameObject.GetComponent<eObject>().GetState("IgnoreProjectile") > 0)
            {
                return;
            }
            OnObjectExit(obj.gameObject.GetComponent<eObject>());
        }
    }
}
