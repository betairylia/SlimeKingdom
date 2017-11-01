#define SLIME_KINGDOM_DEBUG_LINES

using UnityEngine;
using System.Collections;

public enum ControlMode
{
    KeyboardOnly,
    KeyboardMouse
};

public enum SlimeForm
{
    Original,
    Spring,
    Summer,
    Autumn,
    Winter
};

[RequireComponent(typeof(CharacterController))]
public class Slime : Player
{
    public static ControlMode controlMode = ControlMode.KeyboardMouse;

    public GameObject testAttackObject;

    public GameObject m_slimeMesh;
    public AnimationCurve m_AnimCurve_WormLike;

    public float mouseSensitivity = 10.0f, cameraMinTilt = -90f, cameraMaxTilt = 70f;

    private CharacterController m_controller;
    private CameraController cameraCtrl;

    public Transform dialogPos;

    public float maxSpeed = 3, targetFallSpeed = 0, baseMaxSpeed = 3;
    eObject talkTarget;

    //public GameObject rayTestSphere;
    Vector3 walkDirc = new Vector3(0, 0, 0);
    Vector3 lastDirc = new Vector3(0, 0, 0);
    float walkSpeed = 0, lastSpeed = 0, fallSpeed = 0;

    //m_totalGrounded was changed inside Box_MultiCast method. m_totalGrounded is true only if all the ray has intersection with ground. (for reset position after felling)
    bool m_grounded = false, m_totalGrounded = false;

    Vector3 groundNormal;
    Quaternion qGround = Quaternion.identity, qSlime = Quaternion.identity;

    Vector3 lastStandingPos = new Vector3(0, 0, 0);

    public bool autumnOpened {private set; get;}
    public GameObject autumnObject;
    Vector3 windDirc, windBlowEffect;
    bool inWind = false, inCyclone = false;
    Transform cyclonePivot;

    bool isPaused = false;

    float moveAnimFrame_WormLike = 0;

    float mouseRotationX;

    Collector m_collector;

    GameDataKeeper m_dataKeeper;

    //Effects
    [Space]
    [Header("Effects")]
    public ParticleSystem ps_regen;
    public ParticleSystem ps_regenStars, ps_regenSymbols;

    public GameObject fx_getGold, fx_getHealth, fx_getMana;

    // Use this for initialization
    void Start ()
    {
		health = maxHealth;

        SetState("player");

        maxSpeed = baseMaxSpeed;

        m_controller = gameObject.GetComponent<CharacterController>();

        cameraCtrl = GameObject.Find("CameraController").GetComponent<CameraController>();
        cameraCtrl.transform.rotation = transform.rotation;
        qSlime = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        m_UIController = GameObject.Find("UIController").GetComponent<UIController>();

        m_collector = GetComponentInChildren<Collector>();

        m_dataKeeper = GameDataKeeper.GetSingleton();

        SwitchForm(m_dataKeeper.currentForm);

        autumnOpened = false;
        autumnObject.SetActive(false);
	}

    //direction should be normalized.
    bool Box_MultiRaycast(Vector3 startPos, Vector3 direction, float rayLenth, float boxLenth, float boxWidth, int lSegment, int wSegment, out RaycastHit hitInfo, out float dotResult, out Vector3 normal, int layerMask = -1)
    {
        RaycastHit tmpHitInfo;
        bool flag = false;
        hitInfo = new RaycastHit();

        //calculate the local space transformation of the direction
        Quaternion q = Quaternion.FromToRotation(Vector3.up, direction);
        Vector3 eLen = q * Vector3.right;
        Vector3 eWid = q * Vector3.forward;

        normal = Vector3.up;

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

        m_totalGrounded = true;

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
                                    direction, out tmpHitInfo, rayLenth, layerMask, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log("!");
                    flag = true;
                    if((tmp = Vector3.Dot(tmpHitInfo.normal, direction)) < minDotResult)
                    {
                        minDotResult = tmp;
                        hitInfo = tmpHitInfo;

                        normal = tmpHitInfo.normal;
                    }
                }
                else
                {
                    m_totalGrounded = false;
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

        if (Box_MultiRaycast(transform.position + transform.rotation * (0.3f * Vector3.up), transform.rotation * Vector3.down, 0.5f, 0.4f, 0.4f, 2, 2, out hitInfo, out dotResult, out groundNormal))
        {
            if (dotResult < (-Mathf.Cos(45.0f * Mathf.Deg2Rad))) //less than 45 degrees
            {
                m_grounded = true;
            }
            else
            {
                m_grounded = false;
            }
            //if(Vector3.Dot(groundNormal, Vector3.up) < Mathf.Cos(45.0f * Mathf.Deg2Rad))
            //{
            //    m_grounded = true;
            //}
            //else
            //{
            //    m_grounded = false;
            //}
        }
        else
        {
            m_grounded = false;
        }

        return m_grounded;
    }

    void DoGravityAndMove(float gravityAcc, float maxFallSpeed, Vector3 moveDirc, float spd)
    {
        //Debug.Log(m_grounded);

        if (!m_controller.isGrounded)
        //if(!m_grounded)
        {
            fallSpeed += gravityAcc * Time.deltaTime;

            if (fallSpeed > maxFallSpeed)
                fallSpeed = maxFallSpeed;
        }

        if(inWind && autumnOpened || inCyclone)
        {
            fallSpeed += 10.0f * Time.deltaTime;
            fallSpeed = Mathf.Min(0.0f, fallSpeed);
        }

        m_controller.Move(moveDirc * spd * Time.deltaTime + new Vector3(0, -fallSpeed, 0) * Time.deltaTime);

        //if (m_grounded == false && CheckIfGrounded())
        //if (m_grounded == false && m_controller.isGrounded)
        //{
        //    Debug.Log("!");
        //    fallSpeed = 0;
        //}
        //else
        //{
        //    CheckIfGrounded();
        //}
    }

    void Move(Vector3 dirc, float spd)
    {
        //m_slimeMesh.transform.localPosition = new Vector3(0, 0, m_AnimCurve_WormLike.Evaluate(moveAnimFrame_WormLike));
        //moveAnimFrame_WormLike += Time.deltaTime * 0.8f;

        m_controller.Move(dirc * spd * Time.deltaTime);

        //todo: animation
    }

    public void AutumnOpen()
    {
        if(GameDataKeeper.GetSingleton().currentForm == SlimeForm.Autumn)
        {
            autumnOpened = true;
            autumnObject.SetActive(true);
            autumnObject.transform.localScale = new Vector3(0.2f, 0.1f, 0.2f);

            //give slime a "floating" force.
            if(inWind == true)
            {
                fallSpeed = -4;
            }
        }
    }

    public void AutumnOff()
    {
        autumnOpened = false;
    }

    // Update is called once per frame
    void Update ()
    {
		//Debug.Log("MaxSpeed:"+ maxSpeed+" Speed:"+walkSpeed + "Equip:"+ GameDataKeeper.GetSingleton().currentEquipment);
		//Mana Regen
		if (GameDataKeeper.GetSingleton().currentEquipment == EquipmentClass.Moonlight)
		{
			m_dataKeeper.GainMagic(10.0f * Time.deltaTime);
		}
		else
			m_dataKeeper.GainMagic(5.0f * Time.deltaTime);

        //Deal debuffs
        GameDataKeeper.GetSingleton().UpdateBuff(Time.deltaTime);

        maxSpeed = baseMaxSpeed;

        if (m_dataKeeper.HasBuff(BuffClass.GreatTreeBoss_baseAttack_areaEffectSlow))
        {
            health -= 10 * Time.deltaTime;
            //TODO: health < 0
            maxSpeed = baseMaxSpeed * 0.6f;
        }

        if (m_dataKeeper.HasBuff(BuffClass.RegenPotion))
        {
            if(!ps_regen.isPlaying)
            {
                ps_regen.Play();
                ps_regenStars.Play();
                ps_regenSymbols.Play();
            }
            TriggerEvent(this, "heal " + 5 * Time.deltaTime + " 0");
        }
        else
        {
            ps_regen.Stop();
            ps_regenStars.Stop();
            ps_regenSymbols.Stop();
        }

        //Equipment
        if (GameDataKeeper.GetSingleton().currentEquipment == EquipmentClass.King)
        {
            maxSpeed = maxSpeed * 2f;
        }
        if (GameDataKeeper.GetSingleton().currentEquipment == EquipmentClass.Maple)
        {
            maxSpeed = maxSpeed * 1.15f;
        }

        GameDataKeeper.GetSingleton().currentHealth = health;
		GameDataKeeper.GetSingleton().maxHealth = maxHealth;

        CheckIfGrounded();

        //Open autumn object
        if(autumnOpened)
        {
            autumnObject.transform.localScale = Vector3.MoveTowards(autumnObject.transform.localScale, new Vector3(1.5f, 0.75f, 1.5f), 12.0f * Time.deltaTime);
        }
        else
        {
            autumnObject.transform.localScale = Vector3.MoveTowards(autumnObject.transform.localScale, new Vector3(0.2f, 0.1f, 0.2f), 12.0f * Time.deltaTime);

            //Vector3(0.2, 0.2, 0.2).magnitude = 0.34641
            if (autumnObject.transform.localScale.magnitude <= 0.346f)
            {
                autumnObject.SetActive(false);
            }
        }

        if (isPaused == false && m_UIController.isMenuShown() == false)
        {
            //mouseRotationX = mouseRotationX + Input.GetAxis("Mouse X") * mouseSensitivity;
            //float mouseRotationY = transform.localEulerAngles.x

            if(!autumnOpened && m_controller.isGrounded)
            {
                walkDirc = Vector3.zero;
                if (controlMode == ControlMode.KeyboardMouse)
                {
                    walkDirc.x = Input.GetAxis("Horizontal");
                }
                else
                {
                    walkDirc.x = 0;
                }
                walkDirc.z = Input.GetAxis("Vertical");
                walkDirc.Normalize();

                //Stick to ground
                if(Vector3.Angle(Vector3.up, groundNormal) <= 60.0f)
                {
                    qGround = Quaternion.RotateTowards(qGround, Quaternion.FromToRotation(Vector3.up, groundNormal), 120.0f * Time.deltaTime);
                    //qGround = Quaternion.FromToRotation(Vector3.up, groundNormal);
                }

				if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
                {
                    qSlime = Quaternion.Euler(0, cameraCtrl.transform.eulerAngles.y, 0);
                    //mouseRotationX = transform.localEulerAngles.y;

                    walkSpeed += (maxSpeed * 5 * Time.deltaTime);
                    if (walkSpeed > maxSpeed)
                    {
                        walkSpeed = maxSpeed;
                    }
                }
                else if (walkSpeed > 0)
                {
                    walkSpeed -= (maxSpeed * 7 * Time.deltaTime);
                }
                else
                {
                    walkSpeed = 0;
                    //moveAnimFrame_WormLike = 0;
                }

                if(inCyclone)
                {
                    qSlime *= Quaternion.Euler(0, 1440.0f * Time.deltaTime, 0);
                }

                transform.rotation = qSlime;
                m_slimeMesh.transform.rotation = qGround * qSlime;
                walkDirc = transform.rotation * walkDirc;
            }
            else
            {
                //qGround = Quaternion.identity;
                if (CheckIfGrounded() && Vector3.Angle(Vector3.up, groundNormal) <= 60.0f)
                {
                    qGround = Quaternion.RotateTowards(qGround, Quaternion.FromToRotation(Vector3.up, groundNormal), 120.0f * Time.deltaTime);
                    //qGround = Quaternion.FromToRotation(Vector3.up, groundNormal);
                }
                else
                {
                    qGround = Quaternion.RotateTowards(qGround, Quaternion.identity, 120.0f * Time.deltaTime);
                }

                qSlime = Quaternion.Euler(0, cameraCtrl.transform.eulerAngles.y, 0);

                transform.rotation = qSlime;
                m_slimeMesh.transform.rotation = qGround * qSlime;

                if (controlMode == ControlMode.KeyboardMouse)
                {
                    walkDirc += transform.rotation * (5.0f * Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal"));
                }
                else
                {
                    walkDirc += Vector3.zero;
                }
                walkDirc += transform.rotation * (5.0f * Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical"));
                walkDirc.Normalize();

                walkSpeed += (maxSpeed * 5 * Time.deltaTime);
                if (walkSpeed > maxSpeed)
                {
                    walkSpeed = maxSpeed;
                }

                if(inWind)
                {
                    windBlowEffect = Vector3.MoveTowards(windBlowEffect, windDirc.normalized, 10.0f * Time.deltaTime);
                }
                else
                {
                    windBlowEffect = Vector3.MoveTowards(windBlowEffect, Vector3.zero, 20.0f * Time.deltaTime);
                }
            }

            //SHOOTING
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    SlimeAttack();
            //}

            if (Input.GetButtonDown("Jump") && m_controller.isGrounded)
            //if (Input.GetKeyDown(KeyCode.Space) && m_grounded)
            {
                fallSpeed = targetFallSpeed;
            }

            //if (m_controller.isGrounded || autumnOpened)
            ////if(m_grounded || autumnOpened)
            //{
                lastDirc = walkDirc;
                lastSpeed = walkSpeed;
            //}

            if (m_controller.isGrounded)
            //if (m_grounded)
            {
                AutumnOff();
                if (fallSpeed > 2)
                {
                    fallSpeed = 2;
                }
            }

            if(m_totalGrounded)
            {
                lastStandingPos = transform.position;
            }

            //Talking with NPCs
            if (Input.GetButtonDown("Fire2") && m_controller.isGrounded)
            //if (Input.GetButtonDown("Fire2") && m_grounded)
            {
                if(talkTarget != null)
                {
                    FreezePlayer();

                    Vector3 ply = transform.rotation * Vector3.forward;
                    Vector3 face = talkTarget.transform.position - transform.position;
                    ply.y = 0;
                    face.y = 0;

                    transform.rotation = Quaternion.FromToRotation(ply, face) * transform.rotation;

                    TriggerEvent(talkTarget, "beginDialog");
                }
            }
            if (m_controller.isGrounded == false)
            //if (m_grounded == false)
            {
                walkDirc = lastDirc;
                walkSpeed = lastSpeed;
            }

            //Get Item
            if(Input.GetButtonDown("Fire3"))
            {
                if(m_collector.objectList.Count > 0)
                {
                    TriggerEvent(m_collector.objectList[0], "pickUp", 0);
                }
            }

            //Form change
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchForm(SlimeForm.Original);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchForm(SlimeForm.Spring);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchForm(SlimeForm.Summer);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchForm(SlimeForm.Autumn);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SwitchForm(SlimeForm.Winter);
            }

            //Move(walkDirc, walkSpeed);
        }
        else
        {
            walkDirc = Vector3.forward;
            walkSpeed = 0;
        }

        if (inCyclone)
        {
            DoGravityAndMove(14.4f, 4.0f, cyclonePivot.position - transform.position, 20.0f);
            
            lastSpeed = 0;
        }
        else
        {
            if (autumnOpened)
            {
                if (inWind)
                {
                    DoGravityAndMove(14.4f, 4.0f, 0.6f * walkDirc.normalized + (windDirc.magnitude) * 0.2f * 0.6f * windBlowEffect.normalized, walkSpeed);
                }
                else
                {
                    DoGravityAndMove(14.4f, 4.0f, walkDirc, walkSpeed);
                }
            }
            else
            {
                DoGravityAndMove(14.4f, 100.0f, walkDirc + (walkDirc.y + 0.1f) * Vector3.down, walkSpeed);
            }
        }

        //toggle menu
        if(Input.GetButtonDown("Menu") && m_UIController.isMenuShown() == false)
        {
            m_UIController.ShowMenu();
        }
        else if(Input.GetButtonDown("Menu") && m_UIController.isMenuShown() == true)
        {
            m_UIController.CloseMenu();
        }
    }

    bool buffFlag_dmgSlowGT = false;

    private void FixedUpdate()
    {
        if(buffFlag_dmgSlowGT && !m_dataKeeper.HasBuff(BuffClass.GreatTreeBoss_baseAttack_areaEffectSlow))
        {
            m_dataKeeper.AddBuff(BuffClass.GreatTreeBoss_baseAttack_areaEffectSlow);
        }
        if(!buffFlag_dmgSlowGT && m_dataKeeper.HasBuff(BuffClass.GreatTreeBoss_baseAttack_areaEffectSlow))
        {
            m_dataKeeper.RemoveBuff(BuffClass.GreatTreeBoss_baseAttack_areaEffectSlow);
        }

        inWind = false;
        inCyclone = false;

        buffFlag_dmgSlowGT = false;
    }

    [Space]
    public Material[] formMaterials = new Material[5];
    public UIController m_UIController;
    public Transform formPart;

    void SwitchForm(SlimeForm formId)
    {
        m_dataKeeper.SwitchForm(formId);

        foreach (Transform ch in formPart)
        {
            ch.GetComponent<Renderer>().material = formMaterials[(int)m_dataKeeper.currentForm];
        }
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

    public void SlimeAttack()
    {
        Instantiate(testAttackObject, transform.position + transform.rotation * new Vector3(0.0f, 0.5f, 0.75f), transform.rotation);
    }

    public void FreezePlayer()
    {
        isPaused = true;
    }

    public void UnfreezePlayer()
    {
        isPaused = false;
    }

    public bool IsFreezed()
    {
        return isPaused;
    }

    public void GetCrystal()
    {
        m_dataKeeper.GetCrystal();
    }

    public void UseCrystal()
    {
        m_dataKeeper.UseCrystal();
    }

    public void OnFellDown()
    {
        Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHH---");

        transform.position = lastStandingPos;
    }

    public override void BeHealed(float healing, string[] originalSignal, eObject sourceDoHeal = null)
    {
        base.BeHealed(healing, originalSignal, sourceDoHeal);

        //it is not triggered by any buff
        if(originalSignal.Length == 2)
        {
            GameObject obj = Instantiate(fx_getHealth, transform.position, Quaternion.Euler(-90, 0, 0), transform);

            Destroy(obj, 1.0f);
        }
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch (signal[0])
        {
            case "enterTalker":
                talkTarget = source;
                break;
            case "exitTalker":
                if(talkTarget == source)
                {
                    talkTarget = null;
                }
                break;
            case "dialogEnd":
                UnfreezePlayer();
                break;
            case "picked":
                if(signal[1] == "gold")
                {
                    m_dataKeeper.GetGold(1);

                    GameObject obj = Instantiate(fx_getGold, transform.position, Quaternion.Euler(-90, 0, 0), transform);

                    Destroy(obj, 1.0f);
                }
                else if(signal[1] == "mana")
                {
                    m_dataKeeper.GainMagic(2);

                    GameObject obj = Instantiate(fx_getMana, transform.position, Quaternion.Euler(-90, 0, 0), transform);

                    Destroy(obj, 1.0f);
                }
                else if(signal[1] == "heal")
                {
                    TriggerEvent(this, "heal 5");
                }
                else
                {
                    m_dataKeeper.GetItem(signal[1]);
                }
                break;
            case "useCrystal":
                if(m_dataKeeper.crystalCount > 0)
                {
                    m_dataKeeper.UseCrystal();
                    TriggerEvent(source, "Activate");
                }
                break;
            case "inWind":
                inWind = true;
                windDirc = ((Wind)source).windDirc;
                //TODO: dirc
                break;
            case "inCyclone":
                inCyclone = true;
                cyclonePivot = ((Cyclone)source).cyclonePivot;
                break;
            case "felldown":
                OnFellDown();
                break;
            case "hook":
                Vector3 pos = Toolbox.ToVec3(signal[1], signal[2], signal[3]);
                transform.position = pos + 0.7f * Vector3.down;
                break;
            //debuffs
            case "DmgSlow_GT":
                buffFlag_dmgSlowGT = true;
                break;
        }
    }
}
