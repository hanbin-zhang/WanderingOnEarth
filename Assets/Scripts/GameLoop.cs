using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{   
    void Awake() {        
        Manager.Init(this);
    }

    [Photon.Pun.PunRPC]
    void rpcTarget()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
