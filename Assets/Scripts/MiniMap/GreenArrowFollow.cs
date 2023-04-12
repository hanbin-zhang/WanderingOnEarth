using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GreenArrowFollow : MonoBehaviour
{
    public GameObject BlueSphere;
    private void Awake()
    {
        /*if (!transform.parent.GetComponent<PhotonView>().IsMine) this.enabled = false;
        Debug.Log(transform.parent.GetComponent<PhotonView>().IsMine);*/
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!transform.parent.GetComponent<PhotonView>().IsMine) this.gameObject.SetActive(false);
        else BlueSphere.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
