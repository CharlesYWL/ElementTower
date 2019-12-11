using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    [SerializeField]
    GameObject MainCamera;
    [SerializeField]
    GameObject CameraTwo;

    AudioListener MainCameraAudio;
    AudioListener CameraTwoAudio;

    GameObject Fade;

    private void Start()
    {
        MainCameraAudio = MainCamera.GetComponent<AudioListener>();
        CameraTwoAudio = CameraTwo.GetComponent<AudioListener>();

        MainCamera.SetActive(false);
        MainCameraAudio.enabled = false;

        Fade = GameObject.FindGameObjectWithTag("Fade");
    }

    private void SwitchCamera()
    {
        CameraTwo.SetActive(false);
        CameraTwoAudio.enabled = false;

        MainCamera.SetActive(true);
        MainCameraAudio.enabled = true;

        Destroy(Fade);
    }

}
