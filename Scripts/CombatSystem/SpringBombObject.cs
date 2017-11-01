using UnityEngine;
using System.Collections;

public class SpringBombObject : eObject
{
    public GameObject rangeAttacker;
    public float bombDamage = 40.0f, range = 1.5f;
    public Transform flora;

    bool inWind, inCyclone;
    Transform cyclonePivot;
    Vector3 windDirc;
    Rigidbody m_rigidBody;

    // Use this for initialization
    void Start()
    {
        inWind = false;
        m_rigidBody = GetComponent<Rigidbody>();
        //SetState("player");
    }

    float rotateSpd = 0;

    // Update is called once per frame
    void Update()
    {
        if(inCyclone)
        {
            m_rigidBody.useGravity = false;
            m_rigidBody.velocity = Vector3.MoveTowards(m_rigidBody.velocity, (cyclonePivot.position + Vector3.up * 1.7f - transform.position).normalized * 20.0f, 100.0f * Time.deltaTime);

            flora.localScale = Vector3.MoveTowards(flora.localScale, 5.0f * Vector3.one, 13.0f * Time.deltaTime);
            flora.Rotate(0, rotateSpd * Time.deltaTime, 0, Space.Self);

            if (rotateSpd < 720f)
            {
                rotateSpd += 1080f * Time.deltaTime;
            }
            else
            {
                rotateSpd = 720f;
            }
        }
        else
        {
            if (inWind)
            {
                m_rigidBody.useGravity = false;
                m_rigidBody.velocity = Vector3.MoveTowards(m_rigidBody.velocity, windDirc, 13.0f * Time.deltaTime);

                flora.localScale = Vector3.MoveTowards(flora.localScale, 5.0f * Vector3.one, 13.0f * Time.deltaTime);
                flora.Rotate(0, rotateSpd * Time.deltaTime, 0, Space.Self);

                if (rotateSpd < 720f)
                {
                    rotateSpd += 1080f * Time.deltaTime;
                }
                else
                {
                    rotateSpd = 720f;
                }
            }
            else
            {
                m_rigidBody.useGravity = true;

                flora.localScale = Vector3.MoveTowards(flora.localScale, 1.0f * Vector3.one, 13.0f * Time.deltaTime);
                flora.rotation = Quaternion.RotateTowards(flora.rotation, Quaternion.identity, 720f * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        inWind = false;
        inCyclone = false;
    }

    public override void OnSignalRecieved(eObject source, string[] signal)
    {
        base.OnSignalRecieved(source, signal);

        switch(signal[0])
        {
            case "Activate":
                GameObject rangedBomb = Instantiate(rangeAttacker, transform.position, transform.rotation);
                rangedBomb.GetComponent<SpringBomAttack>().SetAttack(bombDamage, range);

                Destroy(gameObject);
                break;

            case "enterWind":
                m_rigidBody.AddForce(150f * Vector3.up);
                break;

            case "inWind":
                inWind = true;
                windDirc = ((Wind)source).windDirc;
                break;

            case "inCyclone":
                inCyclone = true;
                cyclonePivot = ((Cyclone)source).cyclonePivot;
                break;
        }
    }
}
