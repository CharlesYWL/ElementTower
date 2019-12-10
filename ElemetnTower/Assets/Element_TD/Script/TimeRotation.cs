using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRotation : MonoBehaviour
{
    public float RotateSpeedDay = 10.0f;
    public float RotateSpeedNight = 5.5f;
    [SerializeField]
    private GameObject Sun;

    public static bool Day = true;

    private Vector3 RotateDirection;
    private Vector3 Center;

    private float DelayTime;

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
        if (DelayTime >= 0)
        {
            DelayTime -= Time.deltaTime;
        }
        else
        {
            //Day Time
            if (Sun.transform.position.y >= 0)
            {
                transform.RotateAround(Center, RotateDirection, RotateSpeedDay * Time.deltaTime);
                transform.LookAt(Center);
                Day = true;
            }

            //Night Time
            if (Sun.transform.position.y <= 0)
            {
                transform.RotateAround(Center, RotateDirection, (RotateSpeedNight + 2 * WaveController.waveNumber) * Time.deltaTime);
                transform.LookAt(Center);
                Day = false;
            }
        }
    }
}
