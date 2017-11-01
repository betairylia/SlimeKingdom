using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public enum CameraState
{
    CameraOnPlayer,
    CameraOnOther,
    CameraMovingTowards,
    CameraMovingBack,
	CameraReset,
    CameraOnDirector
};

public class CameraController : eObject
{
	public GameObject slime;
	public Texture2D aim;
    public float cameraDistanceMax = 5.0f;
    public float cameraDistance { private set; get; }
	float mouseSensitivity = 10.0f;

    Camera m_camera;
    Vector3 localPosTarget, targetPos;
    Quaternion localRotTarget, targetRot;

    bool onPlayer = true, playerUnfreezed = true;

    float speed = 50.0f;
    float speedDeg = 360.0f;

    CameraState m_camState;

    PlayableDirector m_director;
    UIController m_UIController;

    // Use this for initialization
    void Start ()
    {
		transform.position = slime.transform.position;
        m_camera = gameObject.GetComponentInChildren<Camera>();

        cameraDistance = cameraDistanceMax;

        m_camera.transform.localRotation = Quaternion.identity;
        m_camera.transform.localPosition = cameraDistance * Vector3.back;

        localPosTarget = m_camera.transform.localPosition;
        localRotTarget = m_camera.transform.localRotation;

        m_camState = CameraState.CameraOnPlayer;

        m_UIController = GameObject.Find("UIController").GetComponent<UIController>();
	}

	void OnGUI()
	{
        if(GameDataKeeper.GetSingleton().currentForm == SlimeForm.Summer)
        {
            Rect rect = new Rect(Screen.width * 0.5f - 10f, Screen.height * 0.47f - 10f, 20f, 20f);
            GUI.DrawTexture(rect, aim);
        }
    }

	// LateUpdate is called once per frame after all Update was completed.
    // Camera should move after the player moves.
	void LateUpdate ()
    {
        if (m_camState == CameraState.CameraOnDirector)
        {
            transform.position = Vector3.zero;
        }
        else
        {
            transform.position = slime.transform.position + 1.7f * Vector3.up;
        }

        //calc camera distance
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(transform.position, transform.rotation * Vector3.back, out hitInfo, cameraDistanceMax, -1, QueryTriggerInteraction.Ignore) == true)
        {
            cameraDistance = hitInfo.distance - 0.2f;

            if (cameraDistance <= 0.5f)
            {
                cameraDistance = 0.5f;
            }
        }
        else
        {
            cameraDistance = cameraDistanceMax;
        }

        localPosTarget = cameraDistance * Vector3.back;
        localRotTarget = Quaternion.identity;

        switch (m_camState)
        {
            case CameraState.CameraOnDirector:
            case CameraState.CameraOnOther:

                slime.GetComponent<Slime>().FreezePlayer();

                break;
            case CameraState.CameraMovingTowards:

                //Moving camera towards the target
                m_camera.transform.position = Vector3.MoveTowards(
                    m_camera.transform.position, targetPos, speed * Time.deltaTime);
                m_camera.transform.rotation = Quaternion.RotateTowards(
                    m_camera.transform.rotation, targetRot, speedDeg * Time.deltaTime);

                slime.GetComponent<Slime>().FreezePlayer();

                //State change
                if(Toolbox.VectorEquals(m_camera.transform.position, targetPos) && Toolbox.QuaternionEquals(m_camera.transform.rotation, targetRot))
                {
                    m_camState = CameraState.CameraOnOther;
                }

                break;
            case CameraState.CameraMovingBack:

                //Moving camera back to player
                m_camera.transform.localPosition = Vector3.MoveTowards(
                    m_camera.transform.localPosition, localPosTarget, speed * Time.deltaTime);
                m_camera.transform.localRotation = Quaternion.RotateTowards(
                    m_camera.transform.localRotation, localRotTarget, speedDeg * Time.deltaTime);

                //State change
                if (Toolbox.VectorEquals(m_camera.transform.localPosition, localPosTarget) && Toolbox.QuaternionEquals(m_camera.transform.localRotation, localRotTarget))
                {
                    m_camState = CameraState.CameraOnPlayer;

                    if(m_UIController.IsPlayerUnfreezeable())
                    {
                        playerUnfreezed = true;
                        slime.GetComponent<Slime>().UnfreezePlayer();
                    }
                }

                break;
            case CameraState.CameraOnPlayer:
                
                float mouseRotationX = 0, mouseRotationY = 0;

                if (Slime.controlMode == ControlMode.KeyboardMouse)
                {
                    mouseRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
                    mouseRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity;
                }
                else
                {
                    mouseRotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal") * 1.15f;
                }

                //Debug.Log(mouseRotationY);

                if ((mouseRotationY > 85) && (mouseRotationY < 180))
                    mouseRotationY = 85;
                else if ((mouseRotationY < 320) && (mouseRotationY > 180))
                    mouseRotationY = 320;
                transform.localEulerAngles = new Vector3(mouseRotationY, mouseRotationX, 0);

                m_camera.transform.localPosition = Vector3.MoveTowards(
                   m_camera.transform.localPosition, localPosTarget, 10.0f * Time.deltaTime);
                m_camera.transform.localRotation = Quaternion.RotateTowards(
                    m_camera.transform.localRotation, localRotTarget, 360.0f * Time.deltaTime);

                //if (playerUnfreezed == false && (m_UIController && m_UIController.IsPlayerUnfreezeable()))
                //{
                //    playerUnfreezed = true;
                //    slime.GetComponent<Slime>().UnfreezePlayer();
                //}

			
				if (Input.GetButtonDown ("ResetCam")) 
				{
					m_camState = CameraState.CameraReset;
				}

	            break;

			case CameraState.CameraReset:
				
				transform.rotation = Quaternion.RotateTowards (transform.rotation, slime.transform.rotation, 720.0f * Time.deltaTime);
				if (Toolbox.QuaternionEquals(transform.rotation, slime.transform.rotation)) 
				{
					m_camState = CameraState.CameraOnPlayer;
                }

				break;
        }
	}

    public void cameraMoveTo(Vector3 pos, Quaternion rot)
    {
        if(m_camState == CameraState.CameraOnPlayer)
        {
            speedDeg = Quaternion.Angle(m_camera.transform.rotation, rot);
            speed = Vector3.Distance(m_camera.transform.position, pos);

            slime.GetComponent<Slime>().FreezePlayer();

            playerUnfreezed = false;

            targetPos = pos;
            targetRot = rot;

            m_camState = CameraState.CameraMovingTowards;
        }
    }

    public void cameraMoveBack()
    {
        if(m_camState == CameraState.CameraOnOther || m_camState == CameraState.CameraOnDirector || m_camState == CameraState.CameraMovingTowards)
        {
            m_camState = CameraState.CameraMovingBack;
        }
        TriggerEvent(this, "backToPlayer", 1.0f);
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "backToPlayer":
                if (m_camState != CameraState.CameraOnPlayer)
                {
                    m_camState = CameraState.CameraOnPlayer;
                    m_camera.transform.localPosition = localPosTarget;
                    m_camera.transform.localRotation = localRotTarget;

                    if (m_UIController.IsPlayerUnfreezeable())
                    {
                        playerUnfreezed = true;
                        slime.GetComponent<Slime>().UnfreezePlayer();
                    }
                }
                break;

            case "stopDirector":
                if(m_director)
                {
                    m_director.Stop();
                    m_director = null;
                }

                Vector3 tmppos = m_camera.transform.position;
                Quaternion tmprot = m_camera.transform.rotation;

                m_camState = CameraState.CameraOnOther;
                transform.position = slime.transform.position + 1.7f * Vector3.up;
                transform.rotation = slime.transform.rotation;

                m_camera.transform.position = tmppos;
                m_camera.transform.rotation = tmprot;

                break;
        }
    }

    public void BeginDirector(PlayableDirector director)
    {
        if(m_camState != CameraState.CameraOnDirector)
        {
            m_director = director;
            m_camState = CameraState.CameraOnDirector;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            m_director.Play();
            TriggerEvent(this, "stopDirector", (float)m_director.duration);

        }
    }
}
