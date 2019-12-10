using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private float count = 0f;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("EndScenebgm", SoundType.SOUND);
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        this.transform.position += new Vector3(0f, scrollSpeed, 0f);
        if(count >= 43f)
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
