using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float CameraSpeed = 80.0f;
    public float Boarder = 10.0f;
    public float Toplimit = 30.0f;
    public float Botlimit = -200.0f;
    public float Leftlimit = -75.0f;
    public float Rightlimit = 110.0f;

    public float ScrollSpeed = 3000.0f;
    private Vector2 panLimit = new Vector2(100.0f, 200.0f);
    public float minY = 40.0f;
    public float maxY = 110.0f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= (Screen.height - Boarder) && pos.z <= Toplimit)
        {
            pos.z += CameraSpeed * Time.deltaTime;
            //Debug.Log("TopSide: " + pos.z);
        }

        if (Input.mousePosition.y <= Boarder && pos.z >= Botlimit)
        {
            pos.z -= CameraSpeed * Time.deltaTime;
            //Debug.Log("BotSide: " + pos.z);
        }

        if (Input.mousePosition.x <= Boarder && pos.x >= Leftlimit)
        {
            pos.x -= CameraSpeed * Time.deltaTime;
            //Debug.Log("LeftSide: " + pos.x);
        }

        if (Input.mousePosition.x >= (Screen.width - Boarder) && pos.x <= Rightlimit)
        {
            pos.x += CameraSpeed * Time.deltaTime;
           // Debug.Log("RightSide: " + pos.x);
        }

        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * ScrollSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        
        transform.position = pos;


    }
}
