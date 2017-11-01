using UnityEngine;
using System.Collections;

public class WindController : eObject
{
    public GameObject[] windObj;

    void Start()
    {
        foreach (GameObject wind in windObj)
        {
            wind.SetActive(false);
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                foreach (GameObject wind in windObj)
                {
                    wind.SetActive(true);
                }
                break;
            case "Disable":
                foreach (GameObject wind in windObj)
                {
                    wind.SetActive(false);
                }
                break;
        }
    }
}
