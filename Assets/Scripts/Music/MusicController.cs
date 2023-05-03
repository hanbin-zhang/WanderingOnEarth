using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{ 
    public AudioClip musicLow;
    public AudioClip musicMid;
    public AudioClip musicHigh;

    void Update()
    {
        int greenvalue = Manager.GameObjectManager.GetGlobalGreenValue();
        if (greenvalue <= 100)
        {
            GetComponent<AudioSource>().clip = musicLow;
            //Debug.Log("music low");
        }

        else if(greenvalue >= 500)
        {
            GetComponent<AudioSource>().clip = musicHigh;
            Debug.Log("music high");
        }

        else
        {
            GetComponent<AudioSource>().clip = musicMid;
            Debug.Log("music mid");
        }

        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
