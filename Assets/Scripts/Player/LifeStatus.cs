using Cyan;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LifeStatus : MonoBehaviour
{
    public TMPro.TMP_Text lifeValueDisplay;
    public int lifeValue = 100;
    private bool triggered;
    private int elapseTime;
    private int disasterElapseTime;
    
    public UniversalRendererData RendererData;
    private Blit blit;
    private bool blitState = false;
    private StateLabel state;

    // Start is called before the first frame update
    void Start()
    {
        blit = RendererData.rendererFeatures.OfType<Blit>().FirstOrDefault();
        blit.SetActive(blitState);
        RendererData.SetDirty();
        lifeValueDisplay.text = $"{lifeValue}";
        Invoke("Loop", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Loop()
    { 
        PreProcess();
        ProcessLifeValue();
        ProcessDisaster();
        ProcessRendererEffect();
        PostProcess();        
    }

    private void PreProcess()
    {               
        state = Manager.StateController.GetRegionalStateProperty(transform.position).state;
    }
    private void ProcessLifeValue()
    {        
        if (state == StateLabel.POLLUTED)
        {
            if (elapseTime >= 5)
            {
                lifeValue--;
                elapseTime = 0;
            }
        }
        
        if (state == StateLabel.NORMAL)
        {
            if (elapseTime >= 10)
            {
                lifeValue--;
                elapseTime = 0;
            }
        }
        
        if (state == StateLabel.SAFE)
        {

        }
        elapseTime++;
    }

    private void ProcessDisaster()
    {       
        if (state == StateLabel.POLLUTED)
        {
            if (disasterElapseTime >= 5)
            {
                lifeValue -= 10;
                disasterElapseTime = 0;
            }
        }
        
        if (state == StateLabel.NORMAL)
        {
            if (disasterElapseTime >= 10)
            {
                lifeValue -= 10;
                disasterElapseTime = 0;
            }
        }
        
        if (state == StateLabel.SAFE)
        {

        }
        disasterElapseTime++;
    }

    private void ProcessRendererEffect()
    {               
        bool newBlitState = state switch
        {
            StateLabel.POLLUTED => true,
            StateLabel.NORMAL => true,
            StateLabel.SAFE => false,
            _ => false,
        };
        

        if (newBlitState != blitState)
        {
            blitState = newBlitState;
            blit.SetActive(newBlitState);
            RendererData.SetDirty();
        }
    }

    private void PostProcess()
    {
        if (lifeValue >= 0)
        {
            lifeValueDisplay.text = $"{lifeValue}";
            Invoke("Loop", 1f);
        }
        else
        {
            lifeValueDisplay.text = $"{0}";
            // game over
        }
    }
}