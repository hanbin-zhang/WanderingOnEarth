using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();
    }
}
