using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderAndSave : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        Terrain m_terrain = GetComponentInParent<MassiveDrawer>().m_targetTerrain;
        Camera cam = gameObject.GetComponent<Camera>();
        if(cam)
        {
            m_terrain.drawTreesAndFoliage = false;
            cam.Render();
            cam.enabled = false;
            m_terrain.drawTreesAndFoliage = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
