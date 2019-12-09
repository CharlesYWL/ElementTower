using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRotation : MonoBehaviour
{
    public float RotateSpeedDay = 10.0f;
    public float RotateSpeedNight = 10.0f;
    [SerializeField]
    private GameObject Sun;

    private Vector3 RotateDirection;
    private Vector3 Center;
    // Start is called before the first frame update
    void Start()
    {
        RotateDirection = new Vector3(0f, 0f, 1f);
        Center = new Vector3(20f, -0.5f, -40f);
    }

    // Update is called once per frame
    void Update()
    {
/*        transform.RotateAround(Center, RotateDirection, RotateSpeedDay * Time.deltaTime);
        transform.LookAt(Center);*/

        //Day Time
        if (Sun.transform.position.y >= 0)
        {
            transform.RotateAround(Center, RotateDirection, RotateSpeedDay * Time.deltaTime);
            transform.LookAt(Center);
        }

        //Night Time
        if (Sun.transform.position.y <= 0)
        {
            transform.RotateAround(Center, RotateDirection, RotateSpeedNight * Time.deltaTime);
            transform.LookAt(Center);
        }

    }
}
