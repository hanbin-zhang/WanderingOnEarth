using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeStatus : MonoBehaviour
{
    public TMPro.TMP_Text lifeValueDisplay;
    public int lifeValue = 100;
    private bool triggered;
    private int elapseTime;
    private int disasterElapseTime;

    // Start is called before the first frame update
    void Start()
    {
        lifeValueDisplay.text = $"{lifeValue}";
        Invoke("Timer", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Timer()
    {        
        elapseTime++;
        disasterElapseTime++;
        ProcessLifeValue();
        ProcessDisaster();
        if (lifeValue >= 0)
        {
            lifeValueDisplay.text = $"{lifeValue}";
            Invoke("Timer", 1f);
        }
        else
        {
            lifeValueDisplay.text = $"{0}";
            // game over
        }
        
    }

    private void ProcessLifeValue()
    {
        StateLabel stateLabel = Manager.StateController.GetStateProperty(transform.position).label;
        if (stateLabel == StateLabel.POLLUTED)
        {
            if (elapseTime >= 1)
            {
                lifeValue--;
                elapseTime = 0;
            }
        }
        
        if (stateLabel == StateLabel.NORMAL)
        {
            if (elapseTime >= 3)
            {
                lifeValue--;
                elapseTime = 0;
            }
        }
        
        if (stateLabel == StateLabel.SAFE)
        {

        }
       
    }

    private void ProcessDisaster()
    {
        StateLabel stateLabel = Manager.StateController.GetStateProperty(transform.position).label;
        if (stateLabel == StateLabel.POLLUTED)
        {
            if (disasterElapseTime >= 5)
            {
                lifeValue -= 10;
                disasterElapseTime = 0;
            }
        }
        
        if (stateLabel == StateLabel.NORMAL)
        {
            if (disasterElapseTime >= 10)
            {
                lifeValue -= 10;
                disasterElapseTime = 0;
            }
        }
        
        if (stateLabel == StateLabel.SAFE)
        {

        }
    }

}
