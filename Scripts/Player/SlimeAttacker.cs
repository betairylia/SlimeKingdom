using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttacker : MonoBehaviour
{
    AttackAction actionNow, actionNext;

    public Slime slime;
    public Transform slimeTransform, camCtrlTransform;
    public GameObject attackObject, springBomb, summerWhip, star;

    public float screenPosX = 0.5f, screenPosY = 0.47f;

    public float timeBaseAttack = 0.5f, timeBaseAttackTot = 0.7f, timeBaseAttackLast = 1.0f, radius = 1.0f;
    bool SpringBombThrowed = false;

	// Use this for initialization
	void Start ()
    {
        camCtrlTransform = GameObject.Find("CameraController").transform;
	}

    bool addIntoAttackQueue(AttackAction act)
    {
        if(AttackAction.manaCost[(int)act.type] > GameDataKeeper.GetSingleton().currentMagic)
        {
            //TODO: warning - not enough mana
            return false;
        }

        GameDataKeeper.GetSingleton().UseMagic(AttackAction.manaCost[(int)act.type]);

        if (actionNow == null)
        {
            actionNow = act;
            actionNow.Init();
            return true;
        }
        else
        {
            actionNext = act;
            return true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1") && (!this.GetComponent<Slime>().IsFreezed()))
        {
            if (actionNow == null)
            {
                addIntoAttackQueue(new baseAttack_1(this));
            }
            else
            {
                switch(actionNow.type)
                {
                    case AttackActionType.Base_1:
                        addIntoAttackQueue(new baseAttack_2(this));
                        break;
                    case AttackActionType.Base_2:
                        addIntoAttackQueue(new baseAttack_1(this));
                        break;
                }
            }
        }

        if(Input.GetButtonDown("SpecialAttack") && (!this.GetComponent<Slime>().IsFreezed()))
        {
            switch (GameDataKeeper.GetSingleton().currentForm)
            {
                case SlimeForm.Original:
                    addIntoAttackQueue(new SpecialOrigin(this, screenPosX, screenPosY));
                    break;
                case SlimeForm.Spring:
                    if (SpringBombThrowed == false && (actionNow == null || actionNow.type != AttackActionType.SpecialSpring))
                    {
                        SpringBombThrowed = true;
                        addIntoAttackQueue(new SpecialSpring(this));
                    }
                    else if ((actionNow == null || actionNow.type != AttackActionType.SpecialSpringBoom))
                    {
                        SpringBombThrowed = false;
                        addIntoAttackQueue(new SpecialSpringBoom(this));
                    }
                    break;
                case SlimeForm.Summer:
                    addIntoAttackQueue(new SpecialSummer(this, screenPosX, screenPosY));
                    break;
                case SlimeForm.Autumn:
                    //Use attack action to prevent player open and close autumn ability so quickly.
                    addIntoAttackQueue(new SpecialAutumn(this));
                    break;
                case SlimeForm.Winter:
                    break;
                default:
                    break;
            }
        }

        if(
            (this.GetComponent<Slime>().autumnOpened && (
                                                            !Input.GetButton("SpecialAttack") || 
                                                            GameDataKeeper.GetSingleton().currentForm != SlimeForm.Autumn)) 
            && (!this.GetComponent<Slime>().IsFreezed()))
        {
            GetComponent<Slime>().AutumnOff();
        }

        //do attack action
        if (actionNow != null)
        {
            actionNow.Update(Time.deltaTime);

            if (actionNow.isOver() && actionNext != null && (!this.GetComponent<Slime>().IsFreezed()))
            {
                actionNow = null;
                actionNow = actionNext;
                actionNext = null;

                actionNow.Init();
            }
            else if (actionNow.isEndCombo())
            {
                actionNow = null;
            }
        }
    }
}
