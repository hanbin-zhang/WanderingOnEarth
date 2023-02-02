using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    public float time = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        // Time has run out, do something
        Debug.Log("Time's up!");

        // freeze all move
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
