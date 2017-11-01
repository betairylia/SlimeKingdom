using UnityEngine;
using System.Collections;

public class PressurePlate : eObject
{
    public Material onMat, offMat;
    public eObject target;
    public string onMessage, offMessage;
    bool flag = false, prevFlag = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(flag && !prevFlag)//Enter
        {
            //player enter
            GetComponent<Renderer>().material = onMat;
            TriggerEvent(target, onMessage);
        }
        else if(!flag && prevFlag)//Exit
        {
            //player exit
            GetComponent<Renderer>().material = offMat;
            TriggerEvent(target, offMessage);
        }

        prevFlag = flag;
        flag = false;
    }

    public override void OnObjectStay(eObject obj)
    {
        base.OnObjectStay(obj);

        if(obj.GetState("player") > 0)
        {
            flag = true;
        }
    }
}
