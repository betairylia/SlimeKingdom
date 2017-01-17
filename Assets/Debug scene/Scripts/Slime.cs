//#define SLIME_KINGDOM_DEBUG_LINES

using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
public class Slime : eObject
{
    public GameObject m_slimeMesh, m_cameraAim;
    public AnimationCurve m_AnimCurve_WormLike;

    public float mouseSensitivity = 10.0f, cameraMinTilt = -90f, cameraMaxTilt = 70f;

    private CharacterController m_controller;

    //public GameObject rayTestSphere;
    Vector3 walkDirc = new Vector3(0, 0, 0);
    float walkSpeed = 0, maxSpeed = 3, fallSpeed = 0;
    bool m_grounded = false;

    float moveAnimFrame_WormLike = 0;

	// Use this for initialization
	void Start ()
    {
        m_controller = gameObject.GetComponent<CharacterController>();
        SetState("player");
	}

    //direction should be normalized.
    bool Box_MultiRaycast(Vector3 startPos, Vector3 direction, float rayLenth, float boxLenth, float boxWidth, int lSegment, int wSegment, out RaycastHit hitInfo, out float dotResult, int layerMask = -1)
    {
        RaycastHit tmpHitInfo;
        bool flag = false;
        hitInfo = new RaycastHit();

        //calculate the local space transformation of the direction
        Quaternion q = Quaternion.FromToRotation(Vector3.up, direction);
        Vector3 eLen = q * Vector3.right;
        Vector3 eWid = q * Vector3.forward;

        //calculate each steps
        float lStart = 0, wStart = 0, lStep = 0, wStep = 0, minDotResult = 10000000.0f, tmp = 0f;

        if (lSegment > 1)
        {
            lStart = -(boxLenth / 2.0f);
            lStep = (boxLenth / (lSegment - 1));
        }
        if (wSegment > 1)
        {
            wStart = -(boxWidth / 2.0f);
            wStep = (boxWidth / (wSegment - 1));
        }

        int l, w;
        for (l = 0; l < lSegment; l++) 
        {
            for (w = 0; w < wSegment; w++) 
            {

#if SLIME_KINGDOM_DEBUG_LINES
                Debug.DrawLine(
                    startPos + (lStart + l * lStep) * eLen + (wStart + w * wStep) * eWid,
                    startPos + (lStart + l * lStep) * eLen + (wStart + w * wStep) * eWid + (rayLenth * direction), 
                    Color.red, 2.0f);
#endif

                if (Physics.Raycast(startPos + (lStart + l * lStep) * eLen + (wStart + w * wStep) * eWid,
                                    direction, out tmpHitInfo, rayLenth, layerMask))
                {
                    //Debug.Log("!");
                    flag = true;
                    if((tmp = Vector3.Dot(tmpHitInfo.normal, direction)) < minDotResult)
                    {
                        minDotResult = tmp;
                        hitInfo = tmpHitInfo;
                    }
                }
            }
        }

        dotResult = minDotResult;
        return flag;
    }

    //ab.
    //void Move (Vector3 moveDirc)
    //{
    //    moveDirc.Normalize();
    //    moveDirc = transform.rotation * moveDirc;

    //    RaycastHit hitInfo;
    //    float dotResult;

    //    //Debug.DrawLine(transform.position, transform.position + (0.75f * moveDirc), Color.red, 2.0f);

    //    //if (Physics.Raycast(transform.position, moveDirc, out hitInfo, 0.75f))
    //    //{
    //    //    Debug.Log("Hit");
    //    //    //rayTestSphere.transform.position = hitInfo.point;

    //    //    moveDirc -= Vector3.Dot(hitInfo.normal, moveDirc) * hitInfo.normal;
    //    //}

    //    if(Box_MultiRaycast(transform.position + transform.rotation * (0.2f * Vector3.forward), moveDirc, 0.5f, 0.8f, 0.55f, 3, 3, out hitInfo, out dotResult))
    //    {
    //        //Debug.Log("Hit");

    //        moveDirc -= dotResult * hitInfo.normal;
    //    }

    //    transform.Translate(moveDirc * Time.deltaTime * 3, Space.World);
    //}

    bool CheckIfGrounded()
    {
        RaycastHit hitInfo;
        float dotResult = 200f;

        if (Box_MultiRaycast(transform.position, Vector3.down, 0.3f, 0.4f, 0.4f, 2, 2, out hitInfo, out dotResult))
        {
            if(dotResult < (-Mathf.Cos(45.0f * Mathf.Deg2Rad))) //less than 45 degrees
            {
                m_grounded = true;
            }
            else
            {
                m_grounded = false;
            }
        }
        else
        {
            m_grounded = false;
        }

        return m_grounded;
    }

    void DoGravity(float gravityAcc, float maxFallSpeed)
    {
        //Debug.Log(m_grounded);

        if(!m_controller.isGrounded)
        {
            fallSpeed += gravityAcc * Time.deltaTime;

            if (fallSpeed > maxFallSpeed)
                fallSpeed = maxFallSpeed;

            m_controller.Move(new Vector3(0, -fallSpeed, 0) * Time.deltaTime);
        }

        if (m_grounded == false && CheckIfGrounded())
        {
            //Debug.Log("!");
            fallSpeed = 0;
        }
        else
        {
            CheckIfGrounded();
        }
    }

    void Move(Vector3 dirc, float spd)
    {
        //"蠕动"
        //m_slimeMesh.transform.localPosition = new Vector3(0, 0, m_AnimCurve_WormLike.Evaluate(moveAnimFrame_WormLike));
        //moveAnimFrame_WormLike += Time.deltaTime * 0.8f;

        m_controller.Move(dirc * spd * Time.deltaTime);

        //todo: animation
    }

    // Update is called once per frame
    void Update ()
    {
        float mouseRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseRotationY = m_cameraAim.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (mouseRotationY < 180.0 && mouseRotationY > 80.0)  mouseRotationY = 80.0f;
        if (mouseRotationY > 180.0 && mouseRotationY < 330.0) mouseRotationY = 330.0f;

        //float mouseRotationY = transform.localEulerAngles.x
        transform.localEulerAngles = new Vector3(0, mouseRotationX, 0);
        m_cameraAim.transform.localEulerAngles = new Vector3(mouseRotationY, 0, 0);

        walkDirc.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        walkDirc.z = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        walkDirc.Normalize();
        walkDirc = transform.rotation * walkDirc;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            walkSpeed += (maxSpeed * 5 * Time.deltaTime);
            if(walkSpeed > maxSpeed)
            {
                walkSpeed = maxSpeed;
            }
        }
        else if(walkSpeed > 0)
        {
            walkSpeed -= (maxSpeed * 7 * Time.deltaTime);
        }
        else
        {
            walkSpeed = 0;
            //moveAnimFrame_WormLike = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_grounded)
        {
            fallSpeed = -5;
        }
        
        Move(walkDirc, walkSpeed);

        DoGravity(14.4f, 25.0f);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        //"Solid" trigger
        if (!collider.isTrigger)
        {
            //todo
        }
        //Debug.Log(collider.isTrigger);
    }

    public override void OnSignalRecieved(eObject source, string signal)
    {
        //todo
    }
}
