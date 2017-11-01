using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string pointName;

	// Use this for initialization
	void Start ()
    {
		if(GameDataKeeper.GetSingleton().prevSceneName.Equals(pointName))
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
            GameObject.FindGameObjectWithTag("Player").transform.rotation = transform.rotation;

            Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.eulerAngles);

            GameObject.Find("CameraController").transform.rotation = transform.rotation;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(155f / 255f, 240f / 255f, 208f / 255f, 1.0f);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawSphere(Vector3.zero, 0.5f);
        Gizmos.DrawCube(Vector3.forward * 1.5f, new Vector3(0.5f, 0.5f, 3f));

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.white;
    }
}
