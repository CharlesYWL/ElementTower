using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed = 15f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
