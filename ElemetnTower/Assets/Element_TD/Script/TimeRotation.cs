using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRotation : MonoBehaviour
{
    public float RotateSpeed = 10.0f;

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
        transform.RotateAround(Center, RotateDirection, RotateSpeed * Time.deltaTime);
        transform.LookAt(Center);
    }
}
