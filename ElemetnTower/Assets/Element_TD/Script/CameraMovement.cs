using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float CameraSpeed = 0.0f;
    public float Boarder = 10.0f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= (Screen.height - Boarder))
        {
            //transform.Translate(Vector3.up * Time.deltaTime * CameraSpeed, Space.World);
            pos.z += CameraSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= Boarder)
        {
            pos.z -= CameraSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= Boarder)
        {
            pos.x -= CameraSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x >= (Screen.width - Boarder))
        {
            pos.x += CameraSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
