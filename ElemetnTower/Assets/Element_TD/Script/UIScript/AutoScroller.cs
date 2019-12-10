using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("EndScenebgm", SoundType.SOUND);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0f, scrollSpeed, 0f);
    }
}
