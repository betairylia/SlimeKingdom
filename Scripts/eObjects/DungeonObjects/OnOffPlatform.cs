using UnityEngine;
using System.Collections;

public class OnOffPlatform : eObject
{
    public GameObject obj;

    // Use this for initialization
    void Start()
    {
        obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                obj.SetActive(true);
                break;
        }
    }
}
