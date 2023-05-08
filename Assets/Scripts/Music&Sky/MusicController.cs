using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{ 
    public AudioClip musicLow;
    public AudioClip musicMid;
    public AudioClip musicHigh;
    public Material skybox1;
    public Material skybox2;
    public Material skybox3;

    void Update()
    {
        int greenvalue = Manager.GameObjectManager.GetGlobalGreenValue();
        if (greenvalue <= 100)
        {
            GetComponent<AudioSource>().clip = musicLow;
            RenderSettings.skybox = skybox1;
            Debug.Log("music low");
        }

        else if(greenvalue >= 500)
        {
            GetComponent<AudioSource>().clip = musicHigh;
            RenderSettings.skybox = skybox3;
            Debug.Log("music high");
        }

        else
        {
            GetComponent<AudioSource>().clip = musicMid;
            RenderSettings.skybox = skybox2;
            Debug.Log("music mid");
        }

        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
