using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCam : MonoBehaviour 
{
	public Transform targetTransform;
	public float yOffset = 25;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = targetTransform.position;
		transform.Translate (0, yOffset, 0, Space.World);
	}
}
