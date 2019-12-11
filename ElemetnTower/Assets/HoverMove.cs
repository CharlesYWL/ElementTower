using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMove : MonoBehaviour
{
    // Start is called before the first frame update
    private float time=0f;
    public float upSpeed=10f;
    public float ratateSpeed;
    public float fowardSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        RotateCa();
        transform.position += new Vector3(upSpeed * Time.deltaTime, upSpeed * Time.deltaTime, 0);

        FowardMove();
    }

    private void FowardMove()
    {
        throw new NotImplementedException();
    }

    private void UpMove()
    {

    }

    private void RotateCa()
    {
        throw new NotImplementedException();
    }
}
