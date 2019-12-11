using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime = 1f;
    private float timecount = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timecount += Time.deltaTime;
        if (timecount>=lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
