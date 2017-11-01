using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoRotate : MonoBehaviour
{
    public Vector3 Axis = new Vector3(360, 0, 0);
    public bool global = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Axis * Time.deltaTime, global ? Space.World : Space.Self);
	}
}
