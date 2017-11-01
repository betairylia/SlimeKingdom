using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHelper : MonoBehaviour
{
    RenderTexture tex;
    Camera cam = new Camera();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnDrawGizmosSelected()
    {
        //if(tex == null)
        //{
        //    tex = new RenderTexture(320, 180, 16, RenderTextureFormat.ARGB32);
        //}

        //if(cam == null)
        //{
        //    cam = new Camera();
        //}

        //if(cam)
        //{
        //    cam.transform.position = transform.position;
        //    cam.transform.rotation = transform.rotation;

        //    cam.targetTexture = tex;
        //}

        Gizmos.color = new Color(1, 0, 0, 1.0f);
        Gizmos.DrawSphere(transform.position, 1);

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, 60, 100, 0.3f, 1.6f);

        ////Draw cam view
        //if (cam)
        //{
        //    cam.Render();
        //}
        //Gizmos.DrawGUITexture(new Rect(10, 10, 20, 20), tex);
    }
}
