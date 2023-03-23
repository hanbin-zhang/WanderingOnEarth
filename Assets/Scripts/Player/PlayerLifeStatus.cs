using Cyan;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Photon.Pun;
using System.Threading;

public class PlayerLifeStatus : MonoBehaviour
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
    private SemaphoreSlim rpcSemaphore = new(0);


    // Start is called before the first frame update
    void Start()
    {   
        if (!gameObject.GetComponent<PhotonView>().IsMine)
        {
            enabled = false;
        }
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

    [PunRPC]
    public void GetServerLabel(PhotonMessageInfo info)
    {
        StateLabel label = Manager.StateController.GetStateProperty(transform.position).label;
        gameObject.GetComponent<PhotonView>()
            .RPC(nameof(ServerLabelCallback), info.Sender, label);
    }

    [PunRPC]
    public void ServerLabelCallback(StateLabel label) {
        state = label;
        rpcSemaphore.Release();
    }

    private void PreProcess()
    {
        //state = Manager.StateController.GetStateProperty(transform.position).label;
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        PhotonMessageInfo info = new(PhotonNetwork.LocalPlayer, PhotonNetwork.ServerTimestamp, photonView);
        // Call the RPC method and specify a callback function
        photonView.RPC(nameof(GetServerLabel), RpcTarget.MasterClient);
        //Debug.Log("lock");
        rpcSemaphore.Wait();
    }
    private void ProcessLifeValue()
    {        
        if (state == StateLabel.POLLUTED)
        {
            if (elapseTime >= 1)
            {
                lifeValue--;
                elapseTime = 0;
            }
        }
        
        if (state == StateLabel.NORMAL)
        {
            if (elapseTime >= 3)
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
