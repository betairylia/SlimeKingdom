using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class eObject : MonoBehaviour
{
    public eObject() { }

    public Hashtable eventState = new Hashtable();

    public bool TriggerEvent(eObject target, string signal, float delay = 0, bool showInDebug = true)
    {
        if(target == null)
        {
            return false;
        }

        if(delay == 0)
        {
            if (showInDebug)
            {
                Debug.Log("Event: " + this.name + "(" + this.GetType().ToString() + ")\tTO\t" + target.name + "(" + target.GetType().ToString() + ") :\t\t" + signal);
            }

            target.OnSignalRecieved(this, signal.Split(' '));
            return true;
        }

        StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
        {
            if(target != null)
            {
                if(showInDebug)
                {
                    Debug.Log("Event: " + this.name + "(" + this.GetType().ToString() + ")\tTO\t" + target.name + "(" + target.GetType().ToString() + ") :\t\t" + signal);
                }

                target.OnSignalRecieved(this, signal.Split(' '));
            }
        }, delay));

        return true;
    }

    public virtual void OnSignalRecieved(eObject source, string[] signal) { }

    public bool RegisterEvent(eObject target, string signal, float delay = 0)
    {
        if (target == null)
        {
            return false;
        }

        StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
        {
            if (target != null)
            {
                if (target.eventState.Contains(signal))
                {
                    target.eventState[signal] = (int)target.eventState[signal] + 1;
                }
                else
                {
                    target.eventState.Add(signal, 1);
                }
            }
        }, delay));

        return true;
    }

    public bool RemoveEvent(eObject target, string signal, float delay = 0)
    {
        if (target == null)
        {
            return false;
        }

        StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
        {
            if (target != null)
            {
                if (target.eventState.Contains(signal))
                {
                    target.eventState[signal] = (int)target.eventState[signal] - 1;
                    if ((int)target.eventState[signal] <= 0)
                    {
                        target.eventState.Remove(signal);
                    }
                }
            }
        }, delay));

        return true;
    }

    public int GetState(string stateName)
    {
        if(eventState.Contains(stateName))
        {
            return (int)eventState[stateName];
        }

        return 0;
    }

    public void SetState(string stateName, int stateCount = 1)
    {
        if(eventState.Contains(stateName))
        {
            eventState[stateName] = stateCount;
        }
        else
        {
            eventState.Add(stateName, stateCount);
        }

        return;
    }

    //passive part
    //NB. When using OnTriggerXXX on other classes which has inherited eObject class, call base.OnTriggerXXX(obj) first.
    public virtual void OnTriggerEnter( Collider obj )
    {
        if(obj.gameObject.GetComponent<eObject>() != null)
        {
            OnObjectEnter(obj.gameObject.GetComponent<eObject>());
        }
    }

    public virtual void OnTriggerStay(Collider obj)
    {
        if (obj.gameObject.GetComponent<eObject>() != null)
        {
            OnObjectStay(obj.gameObject.GetComponent<eObject>());
        }
    }

    public virtual void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.GetComponent<eObject>() != null)
        {
            OnObjectExit(obj.gameObject.GetComponent<eObject>());
        }
    }

    public virtual void OnObjectEnter(eObject obj) { }
    public virtual void OnObjectStay(eObject obj) { }
    public virtual void OnObjectExit(eObject obj) { }
}
