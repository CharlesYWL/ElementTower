using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject Sun = null;
    public float RotateSpeedDay = 10.0f;
    public float RotateSpeedNight = 3f;
    public static bool Day = true;

    private Vector3 RotateDirection;
    private Vector3 Center;

    private float DelayTime;
    private float Checkdelay = 3f;

    //Able for delay
    private bool Delayable = false;
    private bool Checkable = false;

    private GameObject[] EnemyList;

    // Start is called before the first frame update
    void Start()
    {
        RotateDirection = new Vector3(0f, 0f, 1f);

        Center = new Vector3(20f, -0.5f, -40f);

        DelayTime = 14;

        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (DelayTime >= 0)
        {
            DelayTime -= Time.deltaTime;
        }
        else
        {
            //Day Time
            if (Sun.transform.position.y >= 0)
            {
                Day = true;
                Delayable = true;
                Checkable = false;

                RotateSpeedNight = 3f;

                transform.RotateAround(Center, RotateDirection, RotateSpeedDay * Time.deltaTime);
                transform.LookAt(Center);

            }

            //Night Time
            if (Sun.transform.position.y <= 0)
            {
                Day = false;

                if (Delayable == true)
                {
                    //Debug.Log("Delay Now");
                    Checkdelay -= Time.deltaTime;
                }

                if (Checkdelay <= 0)
                {
                    //Debug.Log("Now can change speed");
                    Delayable = false;
                    Checkdelay = 3f;
                    Checkable = true;
                }

                if (EnemyList.Length == 0 && Checkable == true)
                {
                    //Debug.Log("No enemy, Change speed");
                    RotateSpeedNight = 20f;
                    Checkable = false;
                    Delayable = false;
                }

                transform.RotateAround(Center, RotateDirection, RotateSpeedNight * Time.deltaTime);
                transform.LookAt(Center);
            }
        }
    }
}
